using Microsoft.EntityFrameworkCore;

namespace Whippersnapper.Data;

public class WhippersnapperContext : DbContext
{
    public WhippersnapperContext(DbContextOptions<WhippersnapperContext> options) : base(options)
    {
    }

    public WhippersnapperContext()
    {
    }

    public static string FilePath => Path.Join(AppDomain.CurrentDomain.BaseDirectory, "data", "whippersnapper.db");

    public DbSet<Transcription> Transcriptions { get; set; } = null!;
}

public class Transcription
{
    public ulong Id { get; set; }
    public string TranscriptionText { get; set; }
    public ulong AuthorId { get; set; }
    public string Author { get; set; }
    public ulong ChannelId { get; set; }
    public ulong GuildId { get; set; }
    public bool ContainsBadWords { get; set; }
    public string GuildName { get; set; }
    public string ChannelName { get; set; }
    public DateTime CreatedAt { get; set; }
}