using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Options;
using NAudio.Wave;
using Whippersnapper.Abstractions;
using Whippersnapper.Configuration;
using Whisper.Runtime;

namespace Whippersnapper.Whisper;

public class Transcriber : ITranscriber
{
    private readonly whisper_context _context;
    private readonly ILogger<Transcriber> _logger;
    private readonly bool _translate;
    private readonly whisper_sampling_strategy _strategy;
    private int _threads;

    public Transcriber(IModelManager modelManager,
        ILogger<Transcriber> logger,
        IOptions<WhipperSnapperConfiguration> options)
    {
        _logger = logger;
        var modelFile = modelManager.GetModelPath(options.Value.ModelFile);

        _translate = options.Value.Translate;
        _context = WhisperRuntime.whisper_init_from_file(modelFile);

        _threads = options.Value.Threads;
        this._strategy = options.Value.Strategy;

        Languages = new Dictionary<int, string>();

        SpecialTokens = new Dictionary<int, string>
        {
            [WhisperRuntime.whisper_token_eot(_context)] = "EOT",
            [WhisperRuntime.whisper_token_sot(_context)] = "SOT",
            [WhisperRuntime.whisper_token_solm(_context)] = "SOLM",
            [WhisperRuntime.whisper_token_prev(_context)] = "PREV",
            [WhisperRuntime.whisper_token_not(_context)] = "NOT",
            [WhisperRuntime.whisper_token_beg(_context)] = "BEG"
        };

        var langMaxId = WhisperRuntime.whisper_lang_max_id();

        for (var i = 0; i < langMaxId; i++)
        {
            Languages[WhisperRuntime.whisper_token_lang(_context, i)] = WhisperRuntime.whisper_lang_str(i);
        }
    }

    private Dictionary<int, string> Languages { get; }

    public Dictionary<int, string> SpecialTokens { get; set; }

    public async Task<TranscriptionResult> Transcribe(string filePath)
    {
        var sw = Stopwatch.StartNew();
        var sb = new StringBuilder();

        using (var state = WhisperRuntime.whisper_init_state(_context))
        {
            var parameters =
                WhisperRuntime.whisper_full_default_params(_strategy);

            await using var waveFileReader = new WaveFileReader(filePath);
            if (waveFileReader.WaveFormat.SampleRate != WhisperRuntime.WHISPER_SAMPLE_RATE)
            {
                throw new InvalidOperationException("Invalid sample rate");
            }

            var buffer = new Memory<float>(new float[GetLengthInFrames(waveFileReader)]);


            foreach (var (chunkIterator, chunk) in ProcessFramesWithBuffer(buffer, waveFileReader))
            {
                _logger.LogInformation($"Processing chunk {chunkIterator}");

                parameters.no_context = chunkIterator == 0;
                parameters.translate = _translate;
                parameters.n_threads = _threads;
                parameters.detect_language = false;
                var test = chunk.ToArray();

                var ret = WhisperRuntime.whisper_full_with_state(_context, state, parameters, test, test.Length);
                if (ret != 0)
                {
                    throw new InvalidOperationException("Failed to process audio");
                }


                var segmentsCount = WhisperRuntime.whisper_full_n_segments_from_state(state);


                for (var i = 0; i < segmentsCount; i++)
                {
                    _logger.LogTrace($"Processing segment {i}");
                    var text = WhisperRuntime.whisper_full_get_segment_text_from_state(state, i);


                    if (text != null)
                    {
                        sb.Append(text);
                    }
                }
            }

            WhisperRuntime.whisper_free_state(state);

        }

        sw.Stop();
        WhisperRuntime.whisper_print_timings(_context);


        var transcription = sb.ToString();
        return new TranscriptionResult { Text = transcription, Elapsed = sw.Elapsed };
    }

    private static float? GetNextFrame(WaveStream stream)
    {
        var bytes = new byte[stream.WaveFormat.BlockAlign];

        var read = stream.Read(bytes, 0, bytes.Length);
        if (read == 0)
        {
            return null;
        }

        if (read < bytes.Length)
        {
            throw new InvalidDataException("Unexpected end of file");
        }

        return BitConverter.ToInt16(bytes, 0) / 32768f;
    }


    private static long GetLengthInFrames(WaveStream stream)
    {
        return stream.Length / stream.BlockAlign;
    }

    private static IEnumerable<(int Index, Memory<float> Buffer)> ProcessFramesWithBuffer(Memory<float> buffer,
        WaveStream stream)
    {
        var i = -1;
        while (stream.Position < stream.Length)
        {
            i++;

            for (var f = 0; f < buffer.Length; f++)
            {
                var frame = GetNextFrame(stream);
                if (!frame.HasValue)
                {
                    yield return (i, buffer[..(f - 1)]);
                    yield break;
                }

                buffer.Span[f] = frame.Value;
            }

            yield return (i, buffer);
        }
    }

}