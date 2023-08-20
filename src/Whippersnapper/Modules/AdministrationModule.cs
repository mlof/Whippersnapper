using Discord;
using Discord.Interactions;
using Humanizer;
using MediatR;
using System.Diagnostics;
using System.Text;
using Whippersnapper.Modules.Administration;

namespace Whippersnapper.Modules;

public class AdministrationModule : InteractionModuleBase
{
    private readonly IMediator _mediator;

    public AdministrationModule(IMediator mediator)
    {
        _mediator = mediator;
    }

    [SlashCommand("usage", "Check the usage of the bot")]
    [DefaultMemberPermissions(GuildPermission.Administrator)]
    public async Task TopUsers()
    {
        var guildId = Context.Guild.Id;
        var response = await _mediator.Send(new TopUsersRequest(guildId));

        var embedBuilder = new EmbedBuilder()
            .WithTitle("Whippersnapper Usage")
            .WithColor(Color.Green);

        foreach (var author in response.Authors)
        {
            embedBuilder.AddField(author.Author, author.Count, true);
        }

        await RespondAsync(embeds: new[] { embedBuilder.Build() });
    }

    [SlashCommand("health", "Check the health of the bot")]
    [DefaultMemberPermissions(GuildPermission.Administrator)]
    public async Task HealthAsync()
    {
        var description = new StringBuilder()
            .AppendLine("Whippersnapper is up and running!");
        // get working set

        using var process = Process.GetCurrentProcess();

        var uptime = DateTime.Now - process.StartTime;
        description.AppendLine($"Threads: {process.Threads.Count}");
        description.AppendLine($"Handles: {process.HandleCount}");
        // humanize uptime 

        description.AppendLine($"Uptime: {uptime.Humanize(3)}");
        var embed = new EmbedBuilder()
            .WithTitle("Whippersnapper Health Check")
            .WithColor(Color.Green)
            .WithDescription(description.ToString())
            .Build();


        await RespondAsync(embeds: new[] { embed });
    }
}