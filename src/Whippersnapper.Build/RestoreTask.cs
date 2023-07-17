using Cake.Common.Diagnostics;
using Cake.Common.Tools.DotNet;
using Cake.Frosting;

namespace Whippersnapper.Build;

[TaskName("Restore")]
public class RestoreTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Information("Restoring...");
        context.DotNetRestore(context.RepositoryRoot.ToString());
    }
}