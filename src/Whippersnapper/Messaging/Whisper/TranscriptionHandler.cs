using MediatR;
using Microsoft.Extensions.Options;
using NAudio.Wave;
using System.Diagnostics;
using System.Text;
using Whippersnapper.Configuration;
using Whisper.net;

namespace Whippersnapper.Messaging.Whisper;

public class TranscriptionHandler : IRequestHandler<TranscriptionRequest, TranscriptionResult>
{
    private readonly ILogger<TranscriptionHandler> _logger;
    private readonly WhisperFactory _factory;
    private readonly int _threads;

    public TranscriptionHandler(
        ILogger<TranscriptionHandler> logger,
        IOptions<WhipperSnapperConfiguration> options,
        WhisperFactory factory)
    {
        _logger = logger;
        this._factory = factory;


        _threads = options.Value.Threads;
    }


    public async Task<TranscriptionResult> Handle(TranscriptionRequest request, CancellationToken cancellationToken)
    {
        var filePath = request.Filepath;

        var sw = Stopwatch.StartNew();
        var sb = new StringBuilder();
        await using var processor = GetProcessor(request);

        await using var waveFileReader = new WaveFileReader(filePath);


        var buffer = new Memory<float>(new float[GetLengthInFrames(waveFileReader)]);


        var segments = new List<SegmentData>();

        foreach (var (chunkIterator, chunk) in ProcessFramesWithBuffer(buffer, waveFileReader))
        {
            _logger.LogInformation("Processing chunk {ChunkIterator}", chunkIterator);

            var test = chunk.ToArray();


            await foreach (var segmentData in processor.ProcessAsync(test, cancellationToken))
            {

                sb.Append(segmentData.Text);
                segments.Add(segmentData);
            }
        }


        sw.Stop();


        var transcription = sb.ToString();
        return new TranscriptionResult { Text = transcription, Elapsed = sw.Elapsed, Segments = segments };
    }

    private WhisperProcessor GetProcessor(TranscriptionRequest request)
    {
        var processorBuilder = _factory.CreateBuilder();
        processorBuilder.WithThreads(_threads);
        if (request.ShouldTranslate)
        {
            processorBuilder.WithTranslate();
        }

        processorBuilder.WithBeamSearchSamplingStrategy();

        processorBuilder.WithLanguageDetection();
        processorBuilder.WithProbabilities();

        return processorBuilder.Build();
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
        _factory.Dispose();
    }
}