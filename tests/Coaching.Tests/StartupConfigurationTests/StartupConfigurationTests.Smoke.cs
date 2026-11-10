using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Coaching.Tests;

public sealed partial class StartupConfigurationTests
{
    [Fact]
    public async Task Development_starts_without_cloudinary_resend_or_business_contact_settings()
    {
        await using var factory = new StartupSmokeFactory();
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { BaseAddress = new("https://localhost") });
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/openapi/v1.json")).StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync("/api/v1/me")).StatusCode);
    }
}
