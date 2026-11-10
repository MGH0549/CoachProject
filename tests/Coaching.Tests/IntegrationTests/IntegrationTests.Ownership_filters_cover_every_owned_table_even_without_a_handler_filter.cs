using Coaching.Application;
using Coaching.Domain;
using Coaching.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Coaching.Tests;

public sealed partial class IntegrationTests
{
    [SqlFact]
    public async Task Ownership_filters_cover_every_owned_table_even_without_a_handler_filter()
    {
        await using var app = new TestApiFactory();
        await app.Initialize();
        var one = await app.AddClient("filter-one@example.test");
        var two = await app.AddClient("filter-two@example.test");
        await app.WithDb(async db =>
        {
            db.Add(new CheckIn { ClientId = one, SubmittedAt = DateTimeOffset.UtcNow, CairoDate = DateOnly.FromDateTime(DateTime.UtcNow), Weight = 80, Energy = 7 });
            db.Add(new CheckIn { ClientId = two, SubmittedAt = DateTimeOffset.UtcNow, CairoDate = DateOnly.FromDateTime(DateTime.UtcNow), Weight = 90, Energy = 7 });
            await db.SaveChangesAsync();
        });
        using var scope = app.Services.CreateScope();
        var actor = new TestActor(one);
        var original = scope.ServiceProvider.GetRequiredService<CoachingDbContext>();
        var options = new DbContextOptionsBuilder<CoachingDbContext>().UseSqlServer(original.Database.GetConnectionString()).Options;
        await using var dbOne = new CoachingDbContext(options, actor, scope.ServiceProvider.GetRequiredService<ICairoTime>());
        Assert.Equal(one, (await dbOne.Set<CheckIn>().SingleAsync()).ClientId);
        actor.Id = two;
        Assert.Equal(two, (await dbOne.Set<CheckIn>().AsNoTracking().SingleAsync()).ClientId);
        foreach (var type in dbOne.Model.GetEntityTypes().Where(x => typeof(ClientEntity).IsAssignableFrom(x.ClrType)))
            Assert.NotEmpty(type.GetDeclaredQueryFilters());
        actor.Id = null;
        Assert.Empty(await dbOne.Set<CheckIn>().ToListAsync());
    }
}
