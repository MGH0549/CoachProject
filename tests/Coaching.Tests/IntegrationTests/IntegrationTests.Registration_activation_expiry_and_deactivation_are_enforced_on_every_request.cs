using System.Net;
using System.Net.Http.Json;
using Coaching.Api;
using Coaching.Application.Auth;
using Coaching.Application.Clients;
using Coaching.Application.Progress;
using Coaching.Domain;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Coaching.Tests;

public sealed partial class IntegrationTests
{
    [SqlFact]
    public async Task Registration_activation_expiry_and_deactivation_are_enforced_on_every_request()
    {
        await using var app = new TestApiFactory();
        await app.Initialize();
        using var visitor = app.CreateClient(new() { BaseAddress = new("https://localhost") });
        visitor.DefaultRequestHeaders.Add("X-Coaching-Client", "Angular");
        var registered = await visitor.PostAsJsonAsync("/api/v1/auth/register", new RegisterRequest("new@example.test", TestApiFactory.Password, "عميل", "201000000002", true));
        Assert.Equal(HttpStatusCode.OK, registered.StatusCode);
        using var client = await app.SignIn("new@example.test");
        using var coach = await app.SignIn(TestApiFactory.CoachEmail);
        var me = await client.GetFromJsonAsync<MyAccountResponse>("/api/v1/me", TestJson.Options);
        Assert.Equal(ClientStatus.Pending, me!.Account.Status);
        Assert.Equal(HttpStatusCode.Forbidden, (await client.GetAsync("/api/v1/me/training")).StatusCode);
        var activate = await coach.PostAsJsonAsync($"/api/v1/coach/clients/{me.Account.Id}/activate", new ActivateRequest("باقة", DateOnly.FromDateTime(DateTime.UtcNow).AddDays(10), me.Account.RowVersion));
        Assert.True(activate.IsSuccessStatusCode, await activate.Content.ReadAsStringAsync());
        Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync("/api/v1/me/training")).StatusCode);
        var active = await activate.Content.ReadFromJsonAsync<AccountResponse>(TestJson.Options);
        var stale = await coach.PostAsJsonAsync($"/api/v1/coach/clients/{me.Account.Id}/deactivate", new VersionRequest(me.Account.RowVersion));
        Assert.Equal(HttpStatusCode.Conflict, stale.StatusCode);
        await app.WithDb(async db =>
        {
            var p = await db.Set<ClientProfile>().SingleAsync(x => x.ClientId == me.Account.Id);
            p.AccessEndsAt = DateTimeOffset.UtcNow.AddMinutes(-1);
            await db.SaveChangesAsync();
        });
        Assert.Equal(HttpStatusCode.Forbidden, (await client.GetAsync("/api/v1/me/progress")).StatusCode);
        var expired = await client.GetFromJsonAsync<MyAccountResponse>("/api/v1/me", TestJson.Options);
        Assert.Equal(ClientStatus.Expired, expired!.Account.Status);
        var reactivate = await coach.PostAsJsonAsync($"/api/v1/coach/clients/{me.Account.Id}/activate", new ActivateRequest("باقة", DateOnly.FromDateTime(DateTime.UtcNow).AddDays(10), expired.Account.RowVersion));
        var current = await reactivate.Content.ReadFromJsonAsync<AccountResponse>(TestJson.Options);
        Assert.True((await coach.PostAsJsonAsync($"/api/v1/coach/clients/{me.Account.Id}/deactivate", new VersionRequest(current!.RowVersion))).IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await client.PostAsJsonAsync("/api/v1/me/check-ins", new CheckInRequest(80, null, null, null, 7, null))).StatusCode);
        Assert.True((await client.GetAsync("/api/v1/me/export")).IsSuccessStatusCode);
    }
}
