using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using Whippersnapper.Abstractions;
using Whippersnapper.Configuration;
using Whippersnapper.Whisper;

namespace Whippersnapper;

internal sealed class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Configuration.SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .AddUserSecrets<Program>();


        var socketConfig = new DiscordSocketConfig
        {
            MessageCacheSize = 100,
            GatewayIntents = GatewayIntents.GuildMessages | GatewayIntents.GuildMessageTyping |
                             GatewayIntents.MessageContent | GatewayIntents.Guilds
        };

        builder.Services.AddSingleton(new DiscordSocketClient(socketConfig));

        // add options 

        builder.Services.Configure<WhipperSnapperConfiguration>(builder.Configuration);
        builder.Services.AddSingleton<ITranscriber, Transcriber>();

        builder.Services.AddHttpClient();

        builder.Services.AddSingleton<IModelManager, ModelManager>();

        builder.Services.AddSingleton<IMessageHandler, MessageHandler>();
        builder.Services.AddHostedService<Worker>();

        var host = builder.Build();
        // ensure directories exist 

        var modelManager = host.Services.GetRequiredService<IOptions<WhipperSnapperConfiguration>>();

        var modelDirectory = modelManager.Value.ModelDirectory;
        if (!Directory.Exists(modelDirectory))
        {
            Directory.CreateDirectory(modelDirectory);
        }

        var filesDirectory = modelManager.Value.FileDirectory;

        if (!Directory.Exists(filesDirectory))
        {
            Directory.CreateDirectory(filesDirectory);
        }


        await host.RunAsync();
    }
}