using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Publish;
using Cake.Frosting;

namespace Whippersnapper.Build;

[TaskName("PublishInstaller")]
[IsDependentOn(typeof(PublishTask))]
public class PublishInstallerTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Information("Publishing Installer...");
        context.DotNetPublish(context.InstallerPath.ToString(), new DotNetPublishSettings
        {
            Configuration = context.MsBuildConfiguration,
            Runtime = context.RuntimeIdentifier,

            NoLogo = true,
            Verbosity = DotNetVerbosity.Detailed
        });

        var artifactDirectory = context.ArtifactPath.Combine(context.Version);

        var installerOutput = context.InstallerPath.Combine("bin").Combine(context.MsBuildConfiguration)
            .Combine("en-US").CombineWithFilePath("Whippersnapper.Installer.msi");

        context.CopyFiles(installerOutput.FullPath, artifactDirectory.FullPath);
    }
}