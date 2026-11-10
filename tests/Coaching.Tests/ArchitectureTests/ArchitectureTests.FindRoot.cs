
namespace Coaching.Tests;

public sealed partial class ArchitectureTests
{
    private static string FindRoot()
    {
        for (var d = new DirectoryInfo(AppContext.BaseDirectory); d is not null; d = d.Parent)
            if (File.Exists(Path.Combine(d.FullName, "Coaching.slnx")))
                return d.FullName;
        throw new DirectoryNotFoundException("Solution root was not found.");
    }
}
