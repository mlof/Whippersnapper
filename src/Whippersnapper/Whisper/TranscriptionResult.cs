namespace Whippersnapper.Whisper;

public record TranscriptionResult
{
    public string Text { get; init; }
    public TimeSpan Elapsed { get; set; }
}