using System.Text;
using NAudio.Wave;
using Whisper.Runtime;

namespace Whippersnapper;

public class Transcriber
{
    private readonly whisper_context _context;
    private readonly whisper_full_params _parameters;

    public Transcriber(string modelFile)
    {
        _parameters = WhisperRuntime.whisper_full_default_params(whisper_sampling_strategy.WHISPER_SAMPLING_GREEDY);
        _context = WhisperRuntime.whisper_init_from_file(modelFile);
    }

    public async Task<string> Transcribe(string filePath)
    {
        var sb = new StringBuilder();
        await using var wavs = new WaveFileReader(filePath);
        if (wavs.WaveFormat.SampleRate != WhisperRuntime.WHISPER_SAMPLE_RATE)
        {
            throw new InvalidOperationException("Invalid sample rate");
        }


        // var buffer = new Memory<float>(new float[ChunkSizeInFrames]);
        var buffer = new Memory<float>(new float[GetLengthInFrames(wavs)]);

        foreach (var (chunki, chunk) in ProcessFramesWithBuffer(buffer, wavs))
        {
            Console.WriteLine($"Processing chunk {chunki}");

            _parameters.no_context = chunki == 0;

            var test = chunk.ToArray();

            // TODO: Update the wrapper to accept a span
            // https://learn.microsoft.com/en-us/dotnet/standard/memory-and-spans/memory-t-usage-guidelines#usage-guidelines
            var ret = WhisperRuntime.whisper_full(_context, _parameters, test, test.Length);
            if (ret != 0)
            {
                throw new InvalidOperationException("Failed to process audio");
            }

            var segmentsCount = WhisperRuntime.whisper_full_n_segments(_context);

            for (var i = 0; i < segmentsCount; i++)
            {
                var text = WhisperRuntime.whisper_full_get_segment_text(_context, i);
                Console.WriteLine($"Segment {i}: {text}");

                sb.Append(text);
            }
        }

        WhisperRuntime.whisper_print_timings(_context);

        return sb.ToString();
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