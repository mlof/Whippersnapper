using Discord;
using Discord.WebSocket;
using MediatR;
using Microsoft.Extensions.Options;
using Whippersnapper.Abstractions;
using Whippersnapper.Configuration;
using Whippersnapper.Interactions;
using Whippersnapper.Messaging.Notifications;

namespace Whippersnapper;

internal class Worker : BackgroundService
{
    private readonly DiscordSocketClient _client;
    private readonly ILogger<Worker> _logger;
    private readonly InteractionHandler _interactionHandler;
    private readonly string _modelFile;
    private readonly IModelManager _modelManager;
    private readonly string? _token;
    private readonly IServiceScopeFactory _serviceScope;

    public Worker(ILogger<Worker> logger,
        IModelManager modelManager,
        DiscordSocketClient client,
        InteractionHandler interactionHandler,
        IOptions<WhipperSnapperConfiguration> options, IServiceScopeFactory serviceScope)
    {
        _logger = logger;

        _logger.LogInformation("Starting worker");
        _modelManager = modelManager;
        _client = client;

        _interactionHandler = interactionHandler;
        _serviceScope = serviceScope;
        _client.Log += LogAsync;
        _client.MessageReceived += MessageReceivedAsync;
        _client.Ready += ClientReady;
        _token = options.Value.BotToken;
        _modelFile = options.Value.ModelFile;

        StatusMessage = options.Value.StatusMessage;
    }

    private async Task ClientReady()
    {
        _logger.LogInformation("Client ready");

        if (!string.IsNullOrWhiteSpace(StatusMessage))
        {
            await _client.SetActivityAsync(new Game(StatusMessage));
        }
    }

    private string? StatusMessage { get; }

    private async Task MessageReceivedAsync(SocketMessage arg)
    {
        if (arg is SocketUserMessage { Flags: MessageFlags.VoiceMessage } socketUserMessage)
        {
            if (arg.Attachments.Count == 0)
            {
                _logger.LogError("Received voice message with no attachment. Are your permissions correct?");

                return;
            }
            else if (arg.Attachments.Count > 1)
            {
                _logger.LogError("Received voice message with more than one attachment. What's going on?");

                return;
            }

            await Mediator.Publish(new VoiceMessageReceivedNotification(socketUserMessage));
        }
        else
        {
            await Mediator.Publish(new MessageReceivedNotification(arg));
        }
    }


    private Task LogAsync(LogMessage arg)
    {
        _logger.LogInformation(arg.ToString());
        return Task.CompletedTask;
    }

    private IMediator Mediator
    {
        get
        {
            var scope = _serviceScope.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IMediator>();
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _modelManager.EnsureModelExists(_modelFile);
        await _interactionHandler.InitializeAsync();
        await _client.LoginAsync(TokenType.Bot, _token);
        await _client.StartAsync();


        while (_client.ConnectionState != ConnectionState.Connected)
        {
            _logger.LogInformation("Waiting for connection...");
            await Task.Delay(1000, stoppingToken);
        }

        _logger.LogInformation("Connected");


        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        _client.Dispose();
    }
}