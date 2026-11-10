using Coaching.Domain;
using Xunit;

namespace Coaching.Tests;

public sealed partial class ArchitectureTests
{
    [Fact]
    public void Domain_does_not_reference_application_infrastructure_or_web()
    {
        var references = typeof(Entity).Assembly.GetReferencedAssemblies().Select(x => x.Name!).ToList();
        Assert.DoesNotContain(references, x => x.StartsWith("Coaching.", StringComparison.Ordinal));
        Assert.DoesNotContain(references, x => x.StartsWith("Microsoft.EntityFrameworkCore", StringComparison.Ordinal) || x.StartsWith("Microsoft.AspNetCore", StringComparison.Ordinal));
    }
}
