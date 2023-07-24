using Whippersnapper.Whisper;

namespace Whippersnapper.Abstractions;

public interface ITranscriber : IDisposable
{
    Task<TranscriptionResult> Transcribe(string filePath);
}