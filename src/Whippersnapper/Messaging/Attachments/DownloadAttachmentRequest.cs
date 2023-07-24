using MediatR;

namespace Whippersnapper.Messaging.Attachments;

public record DownloadAttachmentRequest
    (ulong SocketMessageId, string AttachmentFilename, string AttachmentUrl) : IRequest<DownloadAttachmentResponse>;