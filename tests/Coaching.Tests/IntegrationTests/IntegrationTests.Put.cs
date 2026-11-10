using System.Net.Http.Json;
using Xunit;

namespace Coaching.Tests;

public sealed partial class IntegrationTests
{
    private static async Task<T> Put<T>(HttpClient client, string path, object request)
    {
        using var response = await client.PutAsJsonAsync(path, request);
        Assert.True(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
        return (await response.Content.ReadFromJsonAsync<T>())!;
    }
}
