using Whippersnapper.Whisper;

namespace Whippersnapper.Abstractions;

public interface ITranscriber
{
    Task<TranscriptionResult> Transcribe(string filePath);
}