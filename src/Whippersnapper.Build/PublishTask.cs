using Cake.Common.Diagnostics;
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
        context.DotNetPublish(context.ProjectPath.ToString(), new DotNetPublishSettings
        {
            Configuration = context.MsBuildConfiguration,
            Runtime = context.RuntimeIdentifier,
            OutputDirectory = context.ArtifactPath.Combine(context.Version)
        });
    }
}