using Discord.WebSocket;
using MediatR;

namespace Whippersnapper.Messaging.Notifications;

public record MessageTranscribedNotification : INotification
{
    public MessageTranscribedNotification(SocketUserMessage userMessage, string transcription, bool containsBadWords)
    {
        UserMessage = userMessage;
        Transcription = transcription;
        ContainsBadWords = containsBadWords;
    }

    public SocketUserMessage UserMessage { get; set; }
    public string Transcription { get; init; }

    public bool ContainsBadWords { get; init; }
}