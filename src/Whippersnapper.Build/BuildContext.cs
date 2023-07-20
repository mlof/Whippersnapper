using Cake.Common;
using Cake.Common.IO;
using Cake.Common.Tools.GitVersion;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;

namespace Whippersnapper.Build;

public class BuildContext : FrostingContext
{
    public BuildContext(ICakeContext context)
        : base(context)
    {
        RepositoryRoot = GetRepositoryRoot(context);

        MsBuildConfiguration = context.Argument("configuration", "Release");
        ProjectPath = RepositoryRoot.Combine("src").Combine("Whippersnapper");
        TestPath = RepositoryRoot.Combine("src").Combine("Whippersnapper.Tests");
        InstallerPath = RepositoryRoot.Combine("src").Combine("Whippersnapper.Installer");
        RuntimeIdentifier = context.Argument("runtime", "win-x64");
        ArtifactPath = RepositoryRoot.Combine("artifacts");
        var version = context.GitVersion();


        Version = version.SemVer;
    }

    public string Version { get; set; }

    public DirectoryPath InstallerPath { get; set; }

    public DirectoryPath TestPath { get; set; }

    public DirectoryPath ArtifactPath { get; set; }

    public DirectoryPath ProjectPath { get; set; }

    public string MsBuildConfiguration { get; set; }

    public DirectoryPath RepositoryRoot { get; set; }
    public string RuntimeIdentifier { get; set; }


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