using System.Net.Http.Headers;
using System.Net.Http.Json;
using Coaching.Api;
using Coaching.Application.Auth;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Coaching.Tests;

public sealed partial class TestApiFactory
{
    public async Task<HttpClient> SignIn(string email)
    {
        var client = CreateClient(new WebApplicationFactoryClientOptions { BaseAddress = new Uri("https://localhost"), HandleCookies = false });
        client.DefaultRequestHeaders.Add("X-Coaching-Client", "Angular");
        using var response = await client.PostAsJsonAsync("/api/v1/auth/login", new LoginRequest(email, Password));
        var body = await response.Content.ReadAsStringAsync();
        Assert.True(response.IsSuccessStatusCode, body);
        var session = await response.Content.ReadFromJsonAsync<SessionResponse>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session!.AccessToken);
        if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
            client.DefaultRequestHeaders.Add("Cookie", cookies.First().Split(';')[0]);
        return client;
    }
}
