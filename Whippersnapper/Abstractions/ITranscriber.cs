using Whippersnapper.Whisper;

namespace Whippersnapper.Abstractions;

public interface ITranscriber
{
    Task<Transcriber.TranscriptionResult> Transcribe(string filePath);
}