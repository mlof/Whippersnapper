using Whisper.net;

namespace Whippersnapper.Messaging.Whisper;

public record TranscriptionResult
{
    public string Text { get; init; }
    public TimeSpan Elapsed { get; init; }
    public IEnumerable<SegmentData> Segments { get; init; }
}