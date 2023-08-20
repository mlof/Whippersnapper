using MediatR;
using Microsoft.Extensions.Options;
using Whippersnapper.Configuration;

namespace Whippersnapper.Messaging.Audio;

public class ConvertToWavRequestHandler : IRequestHandler<ConvertToWavRequest, ConvertToWavResponse>
{
    private readonly string _fileDirectory;

    public ConvertToWavRequestHandler(
        IHttpClientFactory httpClientFactory,
        IOptions<WhipperSnapperConfiguration> whipperSnapperConfiguration, IMediator mediator
    )
    {
        _fileDirectory = whipperSnapperConfiguration.Value.FileDirectory;
        Directory.CreateDirectory(_fileDirectory);

    }

    public async Task<ConvertToWavResponse> Handle(ConvertToWavRequest request, CancellationToken cancellationToken)
    {
        var wavFilePath = Path.Join(_fileDirectory,
            Path.GetFileNameWithoutExtension(request.Filepath) + ".wav");
        await AudioConverter.ConvertToWav(request.Filepath, wavFilePath, cancellationToken);
        return new ConvertToWavResponse(wavFilePath);
    }
}