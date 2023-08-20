using Discord.WebSocket;
using MediatR;

namespace Whippersnapper.Messaging.Notifications;

public record VoiceMessageReceivedNotification(SocketUserMessage Message) : INotification;