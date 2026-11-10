
namespace Coaching.Api;

public static class TransportSecurityRegistration
{
    public static IServiceCollection AddTransportSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        // services.AddCors(o => o.AddDefaultPolicy(p => p.WithOrigins(configuration["Platform:AngularOrigin"] ?? "https://localhost:4200")
        //     .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE").WithHeaders("Content-Type", "Authorization", "X-Coaching-Client").AllowCredentials()));
        services.AddHttpsRedirection(o => o.HttpsPort = configuration.GetValue("HttpsPort", 7443));
        services.AddHsts(o => { o.MaxAge = TimeSpan.FromDays(365); o.IncludeSubDomains = true; });
        return services;
    }
}
