using Coaching.Application;
using Coaching.Infrastructure.Persistence;
using Coaching.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Coaching.Tests;

public sealed partial class TestApiFactory
{
    public async Task<string> AddClient(string email, bool active = true)
    {
        using var scope = Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<RequestActor>().IsSystem = true;
        var result = await scope.ServiceProvider.GetRequiredService<IAccounts>().Register(email, Password, "عميل اختبار", "201000000001", default);
        Assert.True(result.IsSuccess, result.Error?.Code);
        if (active)
        {
            var db = scope.ServiceProvider.GetRequiredService<CoachingDbContext>();
            var p = await db.Set<Coaching.Domain.ClientProfile>().SingleAsync(x => x.ClientId == result.Value);
            p.Activate("باقة", DateTimeOffset.UtcNow.AddDays(20), DateTimeOffset.UtcNow);
            await db.SaveChangesAsync();
        }

        return result.Value!;
    }
}
