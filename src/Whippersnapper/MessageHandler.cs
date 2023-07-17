using System.Diagnostics;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using Whippersnapper.Abstractions;
using Whippersnapper.Audio;
using Whippersnapper.Configuration;
using Whisper.Runtime;

namespace Whippersnapper;

public class MessageHandler : IMessageHandler
{
    private readonly List<string> _badWords;
    private readonly string _fileDirectory;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly bool _keepAttachments;
    private readonly ITranscriber _transcriber;
    private readonly bool _debug;
    private readonly whisper_sampling_strategy _strategy;

    public MessageHandler(
        ITranscriber transcriber,
        IHttpClientFactory httpClientFactory,
        IOptions<WhipperSnapperConfiguration> whipperSnapperConfiguration
    )
    {
        _transcriber = transcriber;
        _httpClientFactory = httpClientFactory;
        _fileDirectory = whipperSnapperConfiguration.Value.FileDirectory;
        Directory.CreateDirectory(_fileDirectory);

        _badWords = whipperSnapperConfiguration.Value.BadWords;
        _keepAttachments = whipperSnapperConfiguration.Value.KeepAttachments;
        _debug = whipperSnapperConfiguration.Value.Debug;
        _strategy = whipperSnapperConfiguration.Value.Strategy;
    }

    public async Task HandleMessage(SocketUserMessage socketMessage, CancellationToken cancellationToken)
    {
        var attachment = socketMessage.Attachments.FirstOrDefault();

        if (attachment != null)
        {
            using var typing = socketMessage.Channel.EnterTypingState();

            var sw = Stopwatch.StartNew();
            var fileName = socketMessage.Id + "-" + attachment.Filename;
            var filePath = Path.Join(_fileDirectory, fileName);
            var wavFilePath = Path.Join(_fileDirectory,
                Path.GetFileNameWithoutExtension(fileName) + ".wav");

            await DownloadAttachment(attachment, filePath);

            await AudioConverter.ConvertToWav(filePath, wavFilePath, cancellationToken);

            var transcriptionResult =
                await _transcriber.Transcribe(wavFilePath);
            var content = transcriptionResult.Text;


            var reference = new MessageReference(socketMessage.Id);


            sw.Stop();
            var footer = "Transcribed by Whippersnapper in " + sw.ElapsedMilliseconds + "ms";

            content = SanitizeContent(content);


            var embedBuilder = new EmbedBuilder()
                    .WithAuthor(socketMessage.Author)
                    .WithDescription(content)
                    .WithColor(Color.Blue)
                    .WithFooter(footer)
                ;



            await socketMessage.Channel.SendMessageAsync(embed: embedBuilder.Build(),
                allowedMentions: AllowedMentions.None,
                messageReference: reference);

            if (!_keepAttachments)
            {
                File.Delete(filePath);
                File.Delete(wavFilePath);
            }
        }
    }


    private async Task DownloadAttachment(IAttachment attachment, string filePath)
    {
        using var httpClient = _httpClientFactory.CreateClient();
        Console.WriteLine($"Downloading {attachment.Url} to {filePath}");
        var uri = new Uri(attachment.Url);
        await using var s = await httpClient.GetStreamAsync(uri);


        await using var fs = File.Create(filePath);

        await s.CopyToAsync(fs);
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