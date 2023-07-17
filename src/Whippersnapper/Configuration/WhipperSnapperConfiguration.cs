// ReSharper disable UnusedAutoPropertyAccessor.Global

using Whisper.Runtime;

namespace Whippersnapper.Configuration;

public class WhipperSnapperConfiguration
{
    public string? BotToken { get; init; }
    public bool KeepAttachments { get; init; }
    public string ModelFile { get; init; } = Constants.BaseModel;
    public string? StatusMessage { get; init; }

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public List<string> BadWords { get; init; } = new List<string>();
    public string FileDirectory { get; set; } = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "files");
    public string ModelDirectory { get; set; } = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "models");
    public bool Translate { get; set; } = false;
    public bool Debug { get; set; }
    public whisper_sampling_strategy Strategy { get; set; } = whisper_sampling_strategy.WHISPER_SAMPLING_BEAM_SEARCH;
    public int Threads { get; set; } = 4;
}