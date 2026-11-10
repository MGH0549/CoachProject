using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Coaching.Application.Auth;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Coaching.Tests;

public sealed partial class IntegrationTests
{
    [SqlFact]
    public async Task Missing_client_role_returns_unavailable_without_creating_a_partial_account()
    {
        await using var app = new TestApiFactory();
        await app.Initialize(initializeCoach: false);
        await app.WithDb(db => db.Roles.Where(x => x.NormalizedName == "CLIENT").ExecuteDeleteAsync());
        using var visitor = app.CreateClient(new() { BaseAddress = new("https://localhost") });
        visitor.DefaultRequestHeaders.Add("X-Coaching-Client", "Angular");
        using var response = await visitor.PostAsJsonAsync("/api/v1/auth/register",
            new RegisterRequest("missing-role@example.test", TestApiFactory.Password, "عميل اختبار", "201000000001", true));
        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        Assert.Equal("registration_unavailable", (await response.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("code").GetString());
        await app.WithDb(async db => Assert.False(await db.Users.AnyAsync()));
    }
}
