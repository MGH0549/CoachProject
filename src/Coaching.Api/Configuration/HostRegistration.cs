using Serilog;

namespace Coaching.Api;

public static class HostRegistration
{
    public static WebApplicationBuilder ConfigureLoggingAndLimits(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((_, config) => config.MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", Serilog.Events.LogEventLevel.Information)
            .MinimumLevel.Override("System.Net.Http.HttpClient", Serilog.Events.LogEventLevel.Warning)
            .WriteTo.Console());
        builder.WebHost.ConfigureKestrel(k => k.Limits.MaxRequestBodySize = 9 * 1024 * 1024);
        return builder;
    }
}
