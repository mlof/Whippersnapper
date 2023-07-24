using Microsoft.Extensions.Options;
using NAudio.Wave;
using System.Diagnostics;
using System.Text;
using Whippersnapper.Abstractions;
using Whippersnapper.Configuration;
using Whisper.net;

namespace Whippersnapper.Whisper;

public class Transcriber : ITranscriber
{
    private readonly ILogger<Transcriber> _logger;
    private readonly bool _translate;
    private int _threads;
    private readonly WhisperFactory factory;

    public Transcriber(IModelManager modelManager,
        ILogger<Transcriber> logger,
        IOptions<WhipperSnapperConfiguration> options)
    {
        _logger = logger;
        var modelFile = modelManager.GetModelPath(options.Value.ModelFile);

        _translate = options.Value.Translate;


        _threads = options.Value.Threads;

        Languages = new Dictionary<int, string>();


        this.factory = WhisperFactory.FromPath(modelFile);

    }

    private Dictionary<int, string> Languages { get; }


    public async Task<TranscriptionResult> Transcribe(string filePath)
    {
        var sw = Stopwatch.StartNew();
        var sb = new StringBuilder();
        var processorBuilder = factory.CreateBuilder();
        processorBuilder.WithThreads(_threads);
        if (_translate)
        {
            processorBuilder.WithTranslate();
        }

        await using var waveFileReader = new WaveFileReader(filePath);


        var buffer = new Memory<float>(new float[GetLengthInFrames(waveFileReader)]);

        await using var processor = processorBuilder.Build();


        foreach (var (chunkIterator, chunk) in ProcessFramesWithBuffer(buffer, waveFileReader))
        {
            _logger.LogInformation($"Processing chunk {chunkIterator}");

            var test = chunk.ToArray();


            await foreach (var segmentData in processor.ProcessAsync(test))
            {
                sb.Append(segmentData.Text);
            }
        }


        sw.Stop();


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

    public void Dispose()
    {
        factory.Dispose();
    }
}