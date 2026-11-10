using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Coaching.Tests;

public sealed partial class StartupConfigurationTests
{
    private static WebApplicationBuilder Builder(string environment)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { EnvironmentName = environment });
        builder.Configuration.Sources.Clear();
        builder.Configuration.AddInMemoryCollection();
        return builder;
    }
}
