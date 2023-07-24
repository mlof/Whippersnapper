using MediatR;

namespace Whippersnapper.Messaging.Whisper;

public record TranscriptionRequest(string Filepath, bool ShouldTranslate) : IRequest<TranscriptionResult>;