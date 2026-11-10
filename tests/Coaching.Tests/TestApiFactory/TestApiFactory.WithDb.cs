using Coaching.Infrastructure.Persistence;
using Coaching.Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Coaching.Tests;

public sealed partial class TestApiFactory
{
    public async Task WithDb(Func<CoachingDbContext, Task> action)
    {
        using var scope = Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<RequestActor>().IsSystem = true;
        await action(scope.ServiceProvider.GetRequiredService<CoachingDbContext>());
    }
}
