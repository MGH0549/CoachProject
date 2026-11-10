using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Coaching.Application.Exercises;
using Xunit;

namespace Coaching.Tests;

public sealed partial class IntegrationTests
{
    [SqlFact]
    public async Task Invalid_requests_and_openapi_use_the_controller_contract()
    {
        await using var app = new TestApiFactory();
        await app.Initialize();
        using var coach = await app.SignIn(TestApiFactory.CoachEmail);
        Assert.Equal(HttpStatusCode.BadRequest, (await coach.GetAsync("/api/v1/coach/clients?size=101")).StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, (await coach.PostAsJsonAsync("/api/v1/coach/exercises", new ExerciseRequest("", "", "bad", null, false, null))).StatusCode);
        using var wrongType = await coach.PostAsync("/api/v1/coach/exercises", new StringContent("not-json"));
        Assert.Equal(HttpStatusCode.UnsupportedMediaType, wrongType.StatusCode);
        Assert.Equal("unsupported_media_type", (await wrongType.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("code").GetString());
        using var schema = await coach.GetAsync("/openapi/v1.json");
        Assert.True(schema.IsSuccessStatusCode, await schema.Content.ReadAsStringAsync());
        var doc = await schema.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(doc.GetProperty("paths").TryGetProperty("/api/v1/me/training", out _));
        Assert.Equal("no-store", schema.Headers.CacheControl?.ToString());
    }
}
