using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Coaching.Tests;

public sealed partial class PhotoTests
{
    private sealed class PhotoTransport : HttpMessageHandler
    {
        public string Payload { get; private set; } = "";

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        {
            var content = Assert.IsType<MultipartFormDataContent>(request.Content);
            Payload = await content.ReadAsStringAsync(ct);
            var id = await content.Single(x => x.Headers.ContentDisposition?.Name?.Trim('"') == "public_id").ReadAsStringAsync(ct);
            return new(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(new { public_id = id, type = "authenticated" }), Encoding.UTF8, "application/json")
            };
        }
    }
}
