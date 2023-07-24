using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using Whippersnapper.Abstractions;
using Whippersnapper.Configuration;
using Whippersnapper.Data;

namespace Whippersnapper;

public class Initializer : IDisposable
{
    private readonly IServiceScope _scope;

    public Initializer(IServiceProvider serviceProvider)
    {
        this._scope = serviceProvider.CreateScope();
        var options = _scope.ServiceProvider.GetRequiredService<IOptions<WhipperSnapperConfiguration>>();

        this.Options = options.Value;
    }

    private WhipperSnapperConfiguration Options { get; set; }


    public async Task Initialize()
    {
        Log.Information("Initializing...");
        CreateDirectories();

        await DownloadModel();


        // ensure database exists

        await EnsureDatabaseExists();
    }

    private async Task EnsureDatabaseExists()
    {
        var context = _scope.ServiceProvider.GetRequiredService<WhippersnapperContext>();


        await context.Database.MigrateAsync();
    }

    private async Task DownloadModel()
    {
        var modelManager = _scope.ServiceProvider.GetRequiredService<IModelManager>();

        await modelManager.EnsureModelExists(Options.ModelFile);
    }

    private void CreateDirectories()
    {
        Log.Information("Creating directories");

        Directory.CreateDirectory(Options.FileDirectory);
        Directory.CreateDirectory(Options.ModelDirectory);

        Directory.CreateDirectory(Path.GetDirectoryName(WhippersnapperContext.FilePath));
    }


    public void Dispose()
    {
        _scope.Dispose();
    }
}