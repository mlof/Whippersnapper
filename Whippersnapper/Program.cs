using System.Diagnostics;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace Whippersnapper;

internal class Program
{
    private readonly AudioConverter _audioConverter;
    private readonly DiscordSocketClient _client;
    private readonly string _fileDirectory;
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly string _token;
    private readonly Transcriber _transcriber;
    private readonly WhipperSnapperConfiguration _whipperSnapperConfiguration;

    private Program()
    {
        var socketConfig = new DiscordSocketConfig { MessageCacheSize = 100, GatewayIntents = GatewayIntents.All };
        _client = new DiscordSocketClient(socketConfig);
        _client.Log += LogAsync;
        _client.MessageReceived += MessageReceivedAsync;



        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        _whipperSnapperConfiguration = new WhipperSnapperConfiguration();
        configuration.Bind(_whipperSnapperConfiguration);

        _audioConverter = new AudioConverter();


        ArgumentNullException.ThrowIfNull(_whipperSnapperConfiguration.BotToken);
        _token = _whipperSnapperConfiguration.BotToken;


        _fileDirectory = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "files");

        Directory.CreateDirectory(_fileDirectory);
        var modelDirectory = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "models");


        var modelPath = Path.Join(modelDirectory, _whipperSnapperConfiguration.ModelFile);


        _transcriber = new Transcriber(modelPath);


        _client.SetActivityAsync(new Game(_whipperSnapperConfiguration.StatusMessage));
    }

    private async Task MessageReceivedAsync(SocketMessage arg)
    {
        // set status to typing


        if (arg is SocketUserMessage { Flags: MessageFlags.VoiceMessage } m)
        {

            var attachment = m.Attachments.FirstOrDefault();

            if (attachment != null)
            {
                using var typing = arg.Channel.EnterTypingState();

                var sw = Stopwatch.StartNew();
                var fileName = arg.Id + "-" + attachment.Filename;
                var filePath = Path.Join(_fileDirectory, fileName);
                var wavFilePath = Path.Join(_fileDirectory,
                    Path.GetFileNameWithoutExtension(fileName) + ".wav");

                await DownloadAttachment(attachment, filePath);

                await _audioConverter.ConvertToWav(filePath, wavFilePath);

                var content = await _transcriber.Transcribe(wavFilePath);


                var reference = new MessageReference(arg.Id);


                sw.Stop();
                var footer = "Transcribed by Whippersnapper in " + sw.ElapsedMilliseconds + "ms";

                content = SanitizeContent(content);


                var embedBuilder = new EmbedBuilder()
                    .WithAuthor(arg.Author)
                    .WithDescription(content)
                    .WithColor(Color.Blue)
                    .WithFooter(footer)
                    ;


                await arg.Channel.SendMessageAsync(embed: embedBuilder.Build(), allowedMentions: AllowedMentions.None,
                    messageReference: reference);

                if (!_whipperSnapperConfiguration.KeepAttachments)
                {
                    File.Delete(filePath);
                    File.Delete(wavFilePath);
                }

            }
        }
    }

    private string SanitizeContent(string content)
    {
        var badWords = _whipperSnapperConfiguration.BadWords;

        foreach (var badWord in badWords)
        {
            content = content.Replace(badWord, new string('█', badWord.Length), StringComparison.OrdinalIgnoreCase);
        }

        return content;


    }


    private async Task DownloadAttachment(IAttachment attachment, string filePath)
    {
        Console.WriteLine($"Downloading {attachment.Url} to {filePath}");
        await using var s = await _httpClient.GetStreamAsync(attachment.Url);


        await using var fs = File.Create(filePath);

        await s.CopyToAsync(fs);
    }

    private Task LogAsync(LogMessage arg)
    {
        Console.WriteLine(arg);

        return Task.CompletedTask;
    }

    public static async Task Main(string[] args)
    {
        var modelDirectory = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "models");

        var baseModel = Path.Join(modelDirectory, "ggml-base.bin");

        var baseModelUrl = "https://huggingface.co/ggerganov/whisper.cpp/resolve/main/ggml-base.bin";
        Directory.CreateDirectory(modelDirectory);


        var modelExists = File.Exists(baseModel);
        if (!modelExists)
        {
            Console.WriteLine($"Downloading base model from {baseModelUrl}");
            await using var fs = File.Create(baseModel);

            await using var s = await new HttpClient().GetStreamAsync(baseModelUrl);
            await s.CopyToAsync(fs);




            Console.WriteLine($"Downloaded base model to {baseModel}");
        }

        var program = new Program();

        await program.RunAsync();
    }

    private async Task RunAsync()
    {
        await _client.LoginAsync(TokenType.Bot, _token);
        await _client.StartAsync();


        await Task.Delay(-1);
    }
}