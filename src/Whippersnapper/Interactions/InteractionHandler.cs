using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Whippersnapper.Modules;

namespace Whippersnapper.Interactions;

public class InteractionHandler
{
    private readonly DiscordSocketClient _client;
    private readonly InteractionService _handler;
    private readonly IServiceProvider _services;
    private readonly ILogger _logger;

    public InteractionHandler(DiscordSocketClient client, InteractionService handler, IServiceProvider services,
        IConfiguration config, ILogger<InteractionHandler> logger)
    {
        _client = client;
        _handler = handler;
        _services = services;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        _client.Ready += ReadyAsync;
        _handler.Log += LogAsync;



        await _handler.AddModuleAsync<AdministrationModule>(_services);

        _client.InteractionCreated += HandleInteraction;
    }

    private async Task LogAsync(LogMessage log)
        => Console.WriteLine(log);

    private async Task ReadyAsync()
    {
        var guilds = _client.Guilds.ToList();

        foreach (var guild in guilds)
        {
            _logger.LogInformation("Registering commands for guild {GuildId}: {GuildName}", guild.Id, guild.Name);
            await _handler.RegisterCommandsToGuildAsync(guild.Id, true);

        }
    }

    private async Task HandleInteraction(SocketInteraction interaction)
    {
        try
        {
            var context = new SocketInteractionContext(_client, interaction);

            // Execute the incoming command.
            var result = await _handler.ExecuteCommandAsync(context, _services);

            if (!result.IsSuccess)
                switch (result.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        // implement
                        break;
                    default:
                        break;
                }
        }
        catch
        {
            if (interaction.Type is InteractionType.ApplicationCommand)
                await interaction.GetOriginalResponseAsync()
                    .ContinueWith(async (msg) => await msg.Result.DeleteAsync());
        }
    }
}