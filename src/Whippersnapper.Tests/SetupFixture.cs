using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using Whippersnapper.Configuration;

namespace Whippersnapper.Tests
{
    [SetUpFixture]
    public class SetupFixture
    {
        [OneTimeSetUp]
        public async Task SetupAsync()
        {
            // listen to trace messages

            Trace.Listeners.Add(new ConsoleTraceListener());

            await EnsureBaseModelExists();
        }

        private static async Task EnsureBaseModelExists()
        {
            var modelName = "ggml-base.bin";
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new DebugLoggerProvider())
                ;
            var modelManagerLogger = loggerFactory.CreateLogger<ModelManager>();
            var options = new OptionsWrapper<WhipperSnapperConfiguration>(new WhipperSnapperConfiguration());
            var modelManager = new ModelManager(modelManagerLogger, options);
            await modelManager.EnsureModelExists(modelName);
        }
    }
}