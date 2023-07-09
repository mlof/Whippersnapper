// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Whippersnapper;

public class WhipperSnapperConfiguration
{
    public string? BotToken { get; init; }
    public bool KeepAttachments { get; init; }
    public string? ModelFile { get; init; }
    public string? StatusMessage { get; init; }

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public List<string> BadWords { get; init; } = new List<string>();
}