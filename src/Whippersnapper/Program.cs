using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using Serilog;
using Whippersnapper.Abstractions;
using Whippersnapper.Configuration;
using Whippersnapper.Modules;
using Whippersnapper.Whisper;

namespace Whippersnapper;

internal sealed class Program
{
    public static async Task Main(string[] args)
    {
        var logPath = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "logs", "log_.txt");
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(logPath, rollingInterval: RollingInterval.Day,
                retainedFileTimeLimit: TimeSpan.FromDays(7))
            .WriteTo.Console()
            .CreateLogger();

        var host = BuildHost(args);
        // ensure directories exist 

        Log.Information("Ensuring directories exist");

        var options = host.Services.GetRequiredService<IOptions<WhipperSnapperConfiguration>>();

        var modelDirectory = options.Value.ModelDirectory;
        if (!Directory.Exists(modelDirectory))
        {
            Directory.CreateDirectory(modelDirectory);
        }

        var filesDirectory = options.Value.FileDirectory;

        if (!Directory.Exists(filesDirectory))
        {
            Directory.CreateDirectory(filesDirectory);
        }

        // ensure selected model exists 

        Log.Information("Ensuring model exists");

        var modelManager = host.Services.GetRequiredService<IModelManager>();

        await modelManager.EnsureModelExists(options.Value.ModelFile);

        Log.Information("Starting host");

        await host.RunAsync();
    }

    private static IHost BuildHost(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Configuration.SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .AddUserSecrets<Program>();
        builder.Services.AddWindowsService(options => { options.ServiceName = "Whippersnapper"; });

        builder.Services.AddSerilog();
        var socketConfig = new DiscordSocketConfig
        {
            MessageCacheSize = 100,
            GatewayIntents = GatewayIntents.GuildMessages | GatewayIntents.GuildMessageTyping |
                             GatewayIntents.MessageContent | GatewayIntents.Guilds
        };

        builder.Services.AddSingleton(socketConfig);

        builder.Services.AddSingleton<DiscordSocketClient>();

        var interactionConfig = new InteractionServiceConfig();


        builder.Services.AddSingleton(interactionConfig);
        builder.Services.AddSingleton<InteractionService>();
        builder.Services.AddSingleton<InteractionHandler>();

        // add options 

        builder.Services.Configure<WhipperSnapperConfiguration>(builder.Configuration);
        builder.Services.AddSingleton<ITranscriber, Transcriber>();
        builder.Services.AddSingleton<HealthModule>();
        builder.Services.AddHttpClient();

        builder.Services.AddSingleton<IModelManager, ModelManager>();

        builder.Services.AddSingleton<IMessageHandler, MessageHandler>();
        builder.Services.AddHostedService<Worker>();

        var host = builder.Build();


        return host;
    }
}