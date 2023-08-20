using Discord.WebSocket;
using MediatR;

namespace Whippersnapper.Messaging.Notifications;

public record MessageReceivedNotification(SocketMessage Message) : INotification;