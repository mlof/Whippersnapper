using Cake.Common.Diagnostics;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Test;
using Cake.Frosting;

namespace Whippersnapper.Build;

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
        });
    }
}