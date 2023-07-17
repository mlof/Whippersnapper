using Cake.Core;
using Cake.Frosting;

namespace Whippersnapper.Build;

[TaskName("Default")]
[IsDependentOn(typeof(PublishArtifactsTask))]

public class DefaultTask : FrostingTask
{
    public override void Run(ICakeContext context)
    {
    }
}