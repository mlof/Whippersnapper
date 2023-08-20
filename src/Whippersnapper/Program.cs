using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using Whippersnapper.Abstractions;
using Whippersnapper.Configuration;
using Whippersnapper.Data;
using Whippersnapper.Interactions;
using Whippersnapper.Modules;
using Whisper.net;

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


        await Initialize(host);


        Log.Information("Starting host");


        await host.RunAsync();
    }

    private static async Task Initialize(IHost host)
    {
        using var initializer = new Initializer(host.Services);

        await initializer.Initialize();
    }

    private static IHost BuildHost(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Configuration.SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .AddCommandLine(args);
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
        builder.Services.AddDbContext<WhippersnapperContext>(optionsBuilder =>
        {
            optionsBuilder
                .UseSqlite("Data Source=" + WhippersnapperContext.FilePath)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        });

        builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<Program>());
        // add options 

        builder.Services.Configure<WhipperSnapperConfiguration>(builder.Configuration);
        builder.Services.AddSingleton<AdministrationModule>();
        builder.Services.AddHttpClient();

        builder.Services.AddSingleton(provider =>
        {
            var modelManager = provider.GetRequiredService<IModelManager>();
            var options = provider.GetRequiredService<IOptions<WhipperSnapperConfiguration>>();
            var modelFile = modelManager.GetModelPath(options.Value.ModelFile);

            var factory = WhisperFactory.FromPath(modelFile);

            return factory;
        });

        builder.Services.AddSingleton<IModelManager, ModelManager>();

        builder.Services.AddHostedService<Worker>();

        var host = builder.Build();


        return host;
    }
}