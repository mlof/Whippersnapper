using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Publish;
using Cake.Frosting;

namespace Whippersnapper.Build;

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

        });
        var artifactDirectory = context.ArtifactPath.Combine(context.Version);

        var projectOutputDirectory = context.ProjectPath.Combine("bin").Combine(context.MsBuildConfiguration).Combine("net7.0").Combine("win-x64").Combine("publish");
        context.Zip(projectOutputDirectory, artifactDirectory.CombineWithFilePath("Whippersnapper.zip").FullPath);

    }
}