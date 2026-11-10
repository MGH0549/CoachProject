using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Coaching.Domain;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Coaching.Tests;

public sealed partial class IntegrationTests
{
    [SqlFact]
    public async Task Refresh_rotation_detects_reuse_and_revokes_the_family()
    {
        await using var app = new TestApiFactory();
        await app.Initialize();
        await app.AddClient("refresh@example.test");
        using var browser = await app.SignIn("refresh@example.test");
        using var first = await browser.PostAsync("/api/v1/auth/refresh", null);
        Assert.True(first.IsSuccessStatusCode, await first.Content.ReadAsStringAsync());
        var nextCookie = first.Headers.GetValues("Set-Cookie").First().Split(';')[0];
        using var reused = await browser.PostAsync("/api/v1/auth/refresh", null);
        Assert.Equal(HttpStatusCode.Unauthorized, reused.StatusCode);
        Assert.Equal("refresh_reuse", (await reused.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("code").GetString());
        browser.DefaultRequestHeaders.Remove("Cookie");
        browser.DefaultRequestHeaders.Add("Cookie", nextCookie);
        Assert.Equal(HttpStatusCode.Unauthorized, (await browser.PostAsync("/api/v1/auth/refresh", null)).StatusCode);
        await app.WithDb(async db =>
        {
            var tokens = await db.Set<RefreshToken>().ToListAsync();
            Assert.Equal(2, tokens.Count);
            Assert.All(tokens, t =>
            {
                Assert.NotNull(t.RevokedAt);
                Assert.Equal(64, t.Hash.Length);
            });
        });
    }
}
