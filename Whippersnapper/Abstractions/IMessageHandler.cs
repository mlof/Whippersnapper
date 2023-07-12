using Discord.WebSocket;

namespace Whippersnapper.Abstractions;

internal interface IMessageHandler
{
    Task HandleMessage(SocketUserMessage socketMessage, CancellationToken cancellationToken);
}