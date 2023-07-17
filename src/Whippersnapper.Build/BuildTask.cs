using Cake.Common.Diagnostics;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
using Cake.Frosting;

namespace Whippersnapper.Build;

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
        };
        context.DotNetBuild(context.ProjectPath.ToString(), buildSettings);
        context.DotNetBuild(context.TestPath.ToString(), buildSettings);


    }
}