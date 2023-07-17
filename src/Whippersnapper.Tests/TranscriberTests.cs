using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Extensions.Options;
using System.Runtime.InteropServices;
using Whippersnapper.Configuration;
using Whippersnapper.Whisper;

namespace Whippersnapper.Tests;

public class TranscriberTests
{
    private ModelManager modelManager;
    private ILogger<Transcriber> transcriberLogger;
    private OptionsWrapper<WhipperSnapperConfiguration> options;

    [SetUp]
    public async Task Setup()
    {
        var loggerFactory = new LoggerFactory();
        loggerFactory.AddProvider(new DebugLoggerProvider())
            ;
        var modelManagerLogger = loggerFactory.CreateLogger<ModelManager>();
        this.options = new OptionsWrapper<WhipperSnapperConfiguration>(new WhipperSnapperConfiguration());
        this.transcriberLogger = loggerFactory.CreateLogger<Transcriber>();
        this.modelManager = new ModelManager(modelManagerLogger, options);
        await this.modelManager.EnsureModelExists("ggml-base.bin");
    }

    [Test]
    public async Task CanTranscribe()
    {
        // check if running on linux

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            // skip test if running on linux

            Assert.Ignore("Skipping test on linux");
        }

        var transcriber = new Transcriber(modelManager, transcriberLogger, options);

        var filePath = Path.Combine("files", "default.wav");
        var result = await transcriber.Transcribe(filePath);


        var start =
            @"In a sense, we've come to our nation's capital to cash a check. ";
        var middle =
            @"It is obvious today that America has defaulted on this promise or a note in so far as her citizens of colour are concerned.";

        start = start.Trim().ToLowerInvariant();

        middle = middle.Trim().ToLowerInvariant();
        var actual = result.Text.Trim().ToLowerInvariant();


        actual.Should().StartWithEquivalentOf(start);
        actual.Should().ContainEquivalentOf(middle);
    }
}