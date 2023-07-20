using Discord;
using Discord.Interactions;
using Humanizer;
using System.Diagnostics;
using System.Text;

namespace Whippersnapper.Modules
{
    public class HealthModule : InteractionModuleBase
    {
        public HealthModule()
        {
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
}