namespace Whippersnapper.Abstractions;

public interface IModelManager
{
    Task EnsureModelExists(string modelName = Constants.BaseModel);
    string GetModelPath(string valueModelFile);
}