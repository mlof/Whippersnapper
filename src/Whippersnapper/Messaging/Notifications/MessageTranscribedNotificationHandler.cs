using Discord.WebSocket;
using MediatR;
using Whippersnapper.Data;

namespace Whippersnapper.Messaging.Notifications;

public class MessageTranscribedNotificationHandler : INotificationHandler<MessageTranscribedNotification>
{
    private readonly WhippersnapperContext _dbContext;
    private readonly ILogger<MessageTranscribedNotificationHandler> _logger;

    public MessageTranscribedNotificationHandler(WhippersnapperContext dbContext, DiscordSocketClient discordClient,
        ILogger<MessageTranscribedNotificationHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Handle(MessageTranscribedNotification notification, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notification, nameof(notification));

        var socketUserMessage = notification.UserMessage;

        if (socketUserMessage.Channel is not SocketGuildChannel guildChannel)
        {
            _logger.LogWarning("Message received in non-guild channel. Not saving transcription.");


            return;
        }

        var transcription = new Transcription
        {
            Id = socketUserMessage.Id,
            TranscriptionText = notification.Transcription,
            ContainsBadWords = notification.ContainsBadWords,
            Author = socketUserMessage.Author.ToString(),
            AuthorId = socketUserMessage.Author.Id,

            GuildId = guildChannel.Guild.Id,
            GuildName = guildChannel.Guild.Name,

            ChannelId = guildChannel.Id,
            ChannelName = guildChannel.Name,

            CreatedAt = socketUserMessage.CreatedAt.UtcDateTime,
        };

        // get the guild 


        await _dbContext.Transcriptions.AddAsync(transcription, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Saved transcription for message {MessageId}", socketUserMessage.Id);
    }
}