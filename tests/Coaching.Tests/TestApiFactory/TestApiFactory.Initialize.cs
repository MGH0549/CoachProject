using Coaching.Api;
using Coaching.Infrastructure.Persistence;
using Coaching.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coaching.Tests;

public sealed partial class TestApiFactory
{
    public async Task Initialize(bool initializeCoach = true)
    {
        using var scope = Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<RequestActor>().IsSystem = true;
        await scope.ServiceProvider.GetRequiredService<CoachingDbContext>().Database.MigrateAsync();
        initialized = true;
        if (initializeCoach)
            await Bootstrap.Initialize(scope.ServiceProvider, new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> { ["Bootstrap:Email"] = CoachEmail, ["Bootstrap:Password"] = Password }).Build());
    }
}
