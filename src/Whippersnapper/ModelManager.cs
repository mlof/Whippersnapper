using Microsoft.Extensions.Options;
using Whippersnapper.Abstractions;
using Whippersnapper.Configuration;

namespace Whippersnapper;

public class ModelManager : IModelManager
{
    private readonly ILogger<ModelManager> _logger;
    private readonly string _modelDirectory;

    public ModelManager(ILogger<ModelManager> logger, IOptions<WhipperSnapperConfiguration> options)
    {
        _logger = logger;
        _modelDirectory = options.Value.ModelDirectory;

        if (!Directory.Exists(_modelDirectory))
        {
            Directory.CreateDirectory(_modelDirectory);
        }
    }

    public async Task EnsureModelExists(string modelName = Constants.BaseModel)
    {
        var filePath = Path.Join(_modelDirectory, modelName);

        var baseModelUrl = new Uri("https://huggingface.co/ggerganov/whisper.cpp/resolve/main/" + modelName);
        Directory.CreateDirectory(_modelDirectory);


        var modelExists = File.Exists(filePath);
        if (!modelExists)
        {
            _logger.LogInformation($"Model {modelName} does not exist. Downloading...");
            await using var fs = File.Create(filePath);
            using var client = new HttpClient();
            await using var s = await client.GetStreamAsync(baseModelUrl);
            await s.CopyToAsync(fs);


            _logger.LogInformation($"Downloaded model to {filePath}");
        }
        else
        {
            _logger.LogInformation($"Model {modelName} already exists. Skipping download.");
        }
    }

    public string GetModelPath(string valueModelFile)
    {
        return Path.Join(_modelDirectory, valueModelFile);
    }
}