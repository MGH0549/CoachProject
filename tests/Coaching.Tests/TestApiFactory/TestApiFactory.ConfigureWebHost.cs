using Coaching.Application;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Coaching.Tests;

public sealed partial class TestApiFactory
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var connection = new SqlConnectionStringBuilder(Environment.GetEnvironmentVariable("TestSqlServer"))
        {
            InitialCatalog = databaseName
        };
        builder.UseEnvironment("Development");
        builder.ConfigureAppConfiguration((_, config) => config.AddInMemoryCollection(new Dictionary<string, string?> { ["ConnectionStrings:SqlServer"] = connection.ConnectionString, ["Jwt:Key"] = "integration-tests-key-32-characters-minimum-only", ["Platform:AngularOrigin"] = "https://localhost:4200", ["Platform:CoachWhatsApp"] = "201000000000", ["Platform:PrivacyPolicyUrl"] = "https://localhost:4200/privacy", ["Cloudinary:CloudName"] = "test", ["Cloudinary:ApiKey"] = "test", ["Cloudinary:ApiSecret"] = "test", ["Resend:ApiKey"] = "test", ["Resend:From"] = "coach@example.test", ["Jobs:Enabled"] = "false" }));
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IPhotoStorage>();
            services.AddSingleton<IPhotoStorage>(Photos);
            services.AddDataProtection().UseEphemeralDataProtectionProvider();
            services.RemoveAll<TimeProvider>();
            services.AddSingleton<TimeProvider>(Clock);
            services.AddHttpClient("resend").ConfigurePrimaryHttpMessageHandler(() => Emails);
        });
    }
}
