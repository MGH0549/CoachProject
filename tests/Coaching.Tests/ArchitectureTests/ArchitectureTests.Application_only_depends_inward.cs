using Coaching.Application;
using Xunit;

namespace Coaching.Tests;

public sealed partial class ArchitectureTests
{
    [Fact]
    public void Application_only_depends_inward()
    {
        var references = typeof(IStore).Assembly.GetReferencedAssemblies().Select(x => x.Name!).ToList();
        Assert.DoesNotContain(references, x => x is "Coaching.Api" or "Coaching.Infrastructure" || x.StartsWith("Microsoft.EntityFrameworkCore", StringComparison.Ordinal) || x.StartsWith("Microsoft.AspNetCore", StringComparison.Ordinal));
        Assert.Contains("Coaching.Domain", references);
    }
}
