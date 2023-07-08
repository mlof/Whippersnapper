namespace Whippersnapper;

public class WhipperSnapperConfiguration
{
    public string? BotToken { get; set; }
    public bool KeepAttachments { get; set; }
    public string ModelFile { get; set; }
    public string StatusMessage { get; set; }

    public List<string> BadWords { get; set; } = new List<string>();
}