using Cake.Common.Build;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Frosting;

namespace Whippersnapper.Build;

[TaskName("PublishArtifacts")]
[IsDependentOn(typeof(PublishTask))]
[IsDependentOn(typeof(PublishInstallerTask))]
public class PublishArtifactsTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Information("Publishing Artifacts...");


        var artifactDirectory = context.ArtifactPath.Combine(context.Version);


        if (context.GitHubActions().IsRunningOnGitHubActions)
        {
            var gitHubActions = context.GitHubActions();

            // get all files from the artifact directory

            var files = context.GetFiles(artifactDirectory.CombineWithFilePath("*.*").FullPath);

            foreach (var file in files)
            {
                gitHubActions.Commands.UploadArtifact(file, file.GetFilename().ToString());
            }
        }
    }
}