﻿namespace Whippersnapper.Configuration;

public class WhipperSnapperConfiguration
{
    public string? BotToken { get; init; }
    public bool KeepAttachments { get; init; }
    public string ModelFile { get; init; } = "ggml-base.bin";
    public string? StatusMessage { get; init; }

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public List<string> BadWords { get; init; } = new List<string>();
    public string FileDirectory { get; set; } = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "files");
    public string ModelDirectory { get; set; } = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "models");
    public bool Translate { get; set; } = false;
    public bool Debug { get; set; }
    public int Threads { get; set; } = 4;
}