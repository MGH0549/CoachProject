using Coaching.Infrastructure.Persistence.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Xunit;

namespace Coaching.Tests;

public sealed partial class IntegrationTests
{
    [SqlFact]
    public async Task Role_seeding_is_idempotent_and_preserves_preexisting_role_ids()
    {
        await using var app = new TestApiFactory();
        await app.Initialize(initializeCoach: false);
        await app.WithDb(async db =>
        {
            await db.Roles.ExecuteDeleteAsync();
            db.Roles.AddRange(new IdentityRole { Id = "existing-coach-role", Name = "Coach", NormalizedName = "COACH" },
                new IdentityRole { Id = "existing-client-role", Name = "Client", NormalizedName = "CLIENT" });
            await db.SaveChangesAsync();
            var sql = Assert.IsType<SqlOperation>(Assert.Single(new SeedIdentityRoles().UpOperations)).Sql;
            await db.Database.ExecuteSqlRawAsync(sql);
            await db.Database.ExecuteSqlRawAsync(sql);
            Assert.Equal(2, await db.Roles.CountAsync());
            Assert.Equal("existing-coach-role", (await db.Roles.SingleAsync(x => x.NormalizedName == "COACH")).Id);
            Assert.Equal("existing-client-role", (await db.Roles.SingleAsync(x => x.NormalizedName == "CLIENT")).Id);
        });
    }
}
