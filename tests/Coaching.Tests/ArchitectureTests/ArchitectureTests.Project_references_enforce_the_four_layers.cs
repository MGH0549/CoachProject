using System.Xml.Linq;
using Xunit;

namespace Coaching.Tests;

public sealed partial class ArchitectureTests
{
    [Fact]
    public void Project_references_enforce_the_four_layers()
    {
        var root = FindRoot();
        var expected = new Dictionary<string, string[]>
        {
            ["Domain"] = [],
            ["Application"] = ["Domain"],
            ["Infrastructure"] = ["Application"],
            ["Api"] = ["Infrastructure"]
        };
        foreach (var (project, allowed) in expected)
        {
            var file = Path.Combine(root, "src", "Coaching." + project, "Coaching." + project + ".csproj");
            var references = XDocument.Load(file).Descendants("ProjectReference").Select(x => Path.GetFileNameWithoutExtension(x.Attribute("Include")!.Value).Replace("Coaching.", "", StringComparison.Ordinal)).ToArray();
            Assert.Equal(allowed, references);
        }
    }
}
