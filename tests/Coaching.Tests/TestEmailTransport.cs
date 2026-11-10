using System.Net;
using System.Text.Json;

namespace Coaching.Tests;

public sealed class TestEmailTransport : HttpMessageHandler
{
    public List<JsonElement> Messages { get; } = [];
    public bool Fail { get; set; }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        if (Fail) return new(HttpStatusCode.ServiceUnavailable);
        Messages.Add(JsonSerializer.Deserialize<JsonElement>(await request.Content!.ReadAsStringAsync(ct)));
        return new(HttpStatusCode.OK) { Content = new StringContent("{\"id\":\"test-email\"}") };
    }
}
