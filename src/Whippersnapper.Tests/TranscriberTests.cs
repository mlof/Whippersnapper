using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Extensions.Options;
using Whippersnapper.Configuration;
using Whippersnapper.Messaging.Whisper;
using Whisper.net;

namespace Whippersnapper.Tests;

public class TranscriptionHandlerTests
{
    private OptionsWrapper<WhipperSnapperConfiguration> options;
    private WhisperFactory factory;
    private ILogger<TranscriptionHandler> logger;

    [SetUp]
    public async Task Setup()
    {
        var loggerFactory = new LoggerFactory();
        loggerFactory.AddProvider(new DebugLoggerProvider())
            ;
        this.logger = loggerFactory.CreateLogger<TranscriptionHandler>();
        this.options = new OptionsWrapper<WhipperSnapperConfiguration>(new WhipperSnapperConfiguration());

        var modelManagerLogger = loggerFactory.CreateLogger<ModelManager>();
        var modelManager = new ModelManager(modelManagerLogger, options);
        await modelManager.EnsureModelExists("ggml-base.bin");

        this.factory = WhisperFactory.FromPath(modelManager.GetModelPath("ggml-base.bin"));
    }
    [Test]
    public async Task CanTranscribe()
    {

        var transcriptionHandler = new TranscriptionHandler(logger, options, factory);

        var result = await transcriptionHandler.Handle(new TranscriptionRequest("files/default.wav", true), CancellationToken.None);


        var start =
            @"in a sense";



        result.Text.Trim().Should().StartWithEquivalentOf(start);

    }

}