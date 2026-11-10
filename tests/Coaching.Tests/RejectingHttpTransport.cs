namespace Coaching.Tests;

public sealed class RejectingHttpTransport : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct) =>
        throw new InvalidOperationException("An unconfigured provider must not make an HTTP request.");
}
