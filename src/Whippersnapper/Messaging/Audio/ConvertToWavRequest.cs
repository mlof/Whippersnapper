using MediatR;

namespace Whippersnapper.Messaging.Audio;

public record ConvertToWavRequest(string Filepath) : IRequest<ConvertToWavResponse>
{
}