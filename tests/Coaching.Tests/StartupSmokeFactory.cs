using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coaching.Tests;

public sealed class StartupSmokeFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.ConfigureAppConfiguration((_, configuration) => configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Jwt:Key"] = "startup-smoke-test-only-signing-key-1234",
            ["Cloudinary:CloudName"] = "", ["Cloudinary:ApiKey"] = "", ["Cloudinary:ApiSecret"] = "",
            ["Resend:ApiKey"] = "", ["Resend:From"] = "", ["Platform:CoachWhatsApp"] = "", ["Platform:PrivacyPolicyUrl"] = "", ["Jobs:Enabled"] = "false"
        }));
        builder.ConfigureTestServices(services => services.AddDataProtection().UseEphemeralDataProtectionProvider());
    }
}
