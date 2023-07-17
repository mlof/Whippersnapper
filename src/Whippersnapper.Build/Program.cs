using Cake.Common;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
using Cake.Common.Tools.DotNet.Publish;
using Cake.Common.Tools.DotNet.Test;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;

public static class Program
{
    public static int Main(string[] args)
    {
        return new CakeHost()
            .UseContext<BuildContext>()
            .Run(args);
    }
}

public class BuildContext : FrostingContext
{

    public BuildContext(ICakeContext context)
        : base(context)
    {
        this.RepositoryRoot = GetRepositoryRoot(context);

        this.MsBuildConfiguration = context.Argument("configuration", "Release");
        this.ProjectPath = this.RepositoryRoot.Combine("src").Combine("Whippersnapper");
        this.TestPath = this.RepositoryRoot.Combine("src").Combine("Whippersnapper.Tests");
        this.ArtifactPath = this.RepositoryRoot.Combine("artifacts");


    }

    public DirectoryPath TestPath { get; set; }

    public DirectoryPath ArtifactPath { get; set; }

    public DirectoryPath ProjectPath { get; set; }

    public string MsBuildConfiguration { get; set; }

    public DirectoryPath RepositoryRoot { get; set; }


    private static DirectoryPath GetRepositoryRoot(ICakeContext context)
    {
        var directoryPath = context.Environment.WorkingDirectory;

        while (!context.DirectoryExists(directoryPath.Combine(".git")))
        {
            directoryPath = directoryPath.GetParent();
        }

        return directoryPath;
    }

}


[TaskName("Default")]
[IsDependentOn(typeof(PublishTask))]
public class DefaultTask : FrostingTask
{
    public override void Run(ICakeContext context)
    {
    }
}

[TaskName("Restore")]
public class RestoreTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Information("Restoring...");
        context.DotNetRestore(context.RepositoryRoot.ToString());
    }
}

[TaskName("Build")]
[IsDependentOn(typeof(RestoreTask))]
public class BuildTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Information("Building...");
        var buildSettings = new DotNetBuildSettings()
        {
            Configuration = context.MsBuildConfiguration,
            NoRestore = true,
            NoLogo = true,
        };
        context.DotNetBuild(context.RepositoryRoot.ToString(), buildSettings);

    }
}

[TaskName("Test")]
[IsDependentOn(typeof(BuildTask))]
public class TestTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Information("Testing...");
        context.DotNetTest(context.RepositoryRoot.ToString(), new DotNetTestSettings()
        {
            Configuration = context.MsBuildConfiguration,
            NoRestore = true,
            NoLogo = true,
        });
    }
}

[TaskName("Publish")]
[IsDependentOn(typeof(TestTask))]
public class PublishTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Information("Publishing...");
        context.DotNetPublish(context.ProjectPath.ToString(), new DotNetPublishSettings()
        {
            Configuration = context.MsBuildConfiguration,
            NoRestore = true,
            NoLogo = true,
            OutputDirectory = context.ArtifactPath.Combine("publish")
        });
    }
}