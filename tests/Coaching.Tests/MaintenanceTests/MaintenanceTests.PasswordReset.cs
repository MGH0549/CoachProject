using System.Net;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using Coaching.Application.Auth;
using Microsoft.AspNetCore.WebUtilities;
using Xunit;

namespace Coaching.Tests;

public sealed partial class MaintenanceTests
{
    [SqlFact]
    public async Task Password_reset_uses_Arabic_email_and_revokes_both_access_and_refresh_sessions()
    {
        await using var app = new TestApiFactory(); await app.Initialize(); await app.AddClient("reset@example.test");
        using var browser = await app.SignIn("reset@example.test");
        Assert.True((await browser.PostAsJsonAsync("/api/v1/auth/forgot-password", new ForgotPasswordRequest("reset@example.test"))).IsSuccessStatusCode);
        await RunJobs(app, j => j.SendEmails(default)); Assert.Single(app.Emails.Messages);
        var html = app.Emails.Messages[0].GetProperty("html").GetString()!;
        var url = WebUtility.HtmlDecode(Regex.Match(html, "href=\"([^\"]+)\"").Groups[1].Value);
        var query = QueryHelpers.ParseQuery(new Uri(url).Query);
        var response = await browser.PostAsJsonAsync("/api/v1/auth/reset-password", new ResetPasswordRequest("reset@example.test", query["token"].ToString(), "New-Password-812!"));
        Assert.True(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
        Assert.Equal(HttpStatusCode.Unauthorized, (await browser.GetAsync("/api/v1/me")).StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, (await browser.PostAsync("/api/v1/auth/refresh", null)).StatusCode);
    }
}
