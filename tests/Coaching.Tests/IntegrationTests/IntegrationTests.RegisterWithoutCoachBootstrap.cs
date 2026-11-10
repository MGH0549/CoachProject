using System.Net;
using System.Net.Http.Json;
using Coaching.Api;
using Coaching.Application.Auth;
using Coaching.Domain;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Coaching.Tests;

public sealed partial class IntegrationTests
{
    [SqlFact]
    public async Task Registration_works_on_a_fresh_migrated_database_without_coach_bootstrap()
    {
        await using var app = new TestApiFactory();
        await app.Initialize(initializeCoach: false);
        using var visitor = app.CreateClient(new() { BaseAddress = new("https://localhost") });
        visitor.DefaultRequestHeaders.Add("X-Coaching-Client", "Angular");
        using var registered = await visitor.PostAsJsonAsync("/api/v1/auth/register",
            new RegisterRequest("fresh-register@example.test", TestApiFactory.Password, "عميل اختبار", "201000000001", true));
        Assert.True(registered.StatusCode == HttpStatusCode.OK, await registered.Content.ReadAsStringAsync());
        var id = await registered.Content.ReadFromJsonAsync<string>();
        using var client = await app.SignIn("fresh-register@example.test");
        var account = await client.GetFromJsonAsync<MyAccountResponse>("/api/v1/me", TestJson.Options);
        Assert.Equal(id, account!.Account.Id);
        Assert.Equal(ClientStatus.Pending, account.Account.Status);
        Assert.Equal(HttpStatusCode.Forbidden, (await client.GetAsync("/api/v1/me/training")).StatusCode);
        await app.WithDb(async db =>
        {
            Assert.Equal(1, await db.Users.CountAsync());
            var role = await db.Roles.SingleAsync(x => x.NormalizedName == "CLIENT");
            Assert.True(await db.UserRoles.AnyAsync(x => x.UserId == id && x.RoleId == role.Id));
        });
    }
}
