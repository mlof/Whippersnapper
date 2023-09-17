using System.Diagnostics;
using Discord;
using MediatR;
using Microsoft.Extensions.Options;
using Whippersnapper.Configuration;
using Whippersnapper.Messaging.Attachments;
using Whippersnapper.Messaging.Audio;
using Whippersnapper.Messaging.Whisper;

namespace Whippersnapper.Messaging.Notifications;

public class VoiceMessageReceivedHandler : INotificationHandler<VoiceMessageReceivedNotification>
{
    private readonly List<string> _badWords;
    private readonly IMediator _mediator;
    private readonly bool _shouldTranslate;

    public VoiceMessageReceivedHandler(
        IOptions<WhipperSnapperConfiguration> whipperSnapperConfiguration, IMediator mediator
    )
    {
        _mediator = mediator;

        _badWords = whipperSnapperConfiguration.Value.BadWords;
        _shouldTranslate = whipperSnapperConfiguration.Value.Translate;
    }


    public async Task Handle(VoiceMessageReceivedNotification notification, CancellationToken cancellationToken)
    {
        var socketMessage = notification.Message;

        var attachment = socketMessage.Attachments.Single();

        using var typing = socketMessage.Channel.EnterTypingState();

        var sw = Stopwatch.StartNew();


        var downloadAttachmentResponse = await _mediator.Send(
            new DownloadAttachmentRequest(socketMessage.Id, attachment.Filename, attachment.Url),
            cancellationToken);

        var convertToWavResponse =
            await _mediator.Send(new ConvertToWavRequest(downloadAttachmentResponse.FilePath), cancellationToken);

        var transcriptionResult = await _mediator.Send(
            new TranscriptionRequest(convertToWavResponse.Filepath, _shouldTranslate),
            cancellationToken);


        var originalContent = transcriptionResult.Text;

        var reference = new MessageReference(socketMessage.Id);


        sw.Stop();


        var containsBadWords = ContainsBadWords(originalContent);

        var elapsed = sw.ElapsedMilliseconds;

        var author = socketMessage.Author;
        var embed = CreateEmbed(originalContent, author, elapsed, containsBadWords);


        await socketMessage.Channel.SendMessageAsync(embed: embed,
            allowedMentions: AllowedMentions.None,
            messageReference: reference);

        await _mediator.Publish(
            new MessageTranscribedNotification(socketMessage, originalContent, containsBadWords), cancellationToken);
    }

    private Embed CreateEmbed(string originalContent, IUser author, long elapsed, bool containsBadWords)
    {
        var embedBuilder = new EmbedBuilder()
                .WithAuthor(author)
                .WithFooter("Transcribed by Whippersnapper in " + elapsed + "ms")
                .WithColor(Color.Blue)
            ;

        var content = originalContent;
        if (containsBadWords)
        {
            embedBuilder.WithDescription(SanitizeContent(content)).WithColor(Color.Red);
        }
        else
        {
            embedBuilder.WithDescription(content);
        }


        var embed = embedBuilder.Build();
        return embed;
    }

    private bool ContainsBadWords(string content)
    {
        return _badWords.Any(badWords => content.Contains(badWords, StringComparison.OrdinalIgnoreCase));
    }

    private string SanitizeContent(string content)
    {
        foreach (var badWord in _badWords)
        {
            content = content.Replace(badWord, new string('█', badWord.Length), StringComparison.OrdinalIgnoreCase);
        }

        return content;
    }
}