using MediatR;
using Microsoft.Extensions.Options;
using Whippersnapper.Configuration;

namespace Whippersnapper.Messaging.Attachments;

public class DownloadAttachmentRequestHandler : IRequestHandler<DownloadAttachmentRequest, DownloadAttachmentResponse>
{
    private readonly string _fileDirectory;
    private readonly IHttpClientFactory _httpClientFactory;

    public DownloadAttachmentRequestHandler(IHttpClientFactory httpClientFactory,
        IOptions<WhipperSnapperConfiguration> whipperSnapperConfiguration
    )
    {
        _httpClientFactory = httpClientFactory;

        _fileDirectory = whipperSnapperConfiguration.Value.FileDirectory;
    }

    public async Task<DownloadAttachmentResponse> Handle(DownloadAttachmentRequest request,
        CancellationToken cancellationToken)
    {
        var fileName = request.SocketMessageId + "-" + request.AttachmentFilename;

        var filePath = Path.Join(_fileDirectory, fileName);
        using var httpClient = _httpClientFactory.CreateClient();
        var uri = new Uri(request.AttachmentUrl);
        await using var s = await httpClient.GetStreamAsync(uri, cancellationToken);


        await using var fs = File.Create(filePath);

        await s.CopyToAsync(fs, cancellationToken);

        return new DownloadAttachmentResponse(filePath);
    }
}