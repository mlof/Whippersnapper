namespace Whippersnapper.Abstractions;

public interface IModelManager
{
    Task EnsureModelExists(string modelName);
    string GetModelPath(string valueModelFile);
}