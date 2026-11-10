using Coaching.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Coaching.Tests;

public sealed partial class TestApiFactory
{
    public override async ValueTask DisposeAsync()
    {
        if (initialized)
        {
            using var scope = Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<CoachingDbContext>();
            Assert.Equal(databaseName, db.Database.GetDbConnection().Database);
            Assert.StartsWith("CoachingTests_", databaseName);
            await db.Database.EnsureDeletedAsync();
        }

        await base.DisposeAsync();
    }
}
