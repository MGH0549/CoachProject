using System.Security.Cryptography;

namespace Coaching.Api;

public static class DevelopmentConfiguration
{
    public static WebApplicationBuilder AddDevelopmentDefaults(this WebApplicationBuilder builder)
    {
        if (!builder.Environment.IsDevelopment()) return builder;
        if (string.IsNullOrWhiteSpace(builder.Configuration["Jwt:Key"]))
            builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
            { ["Jwt:Key"] = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)) });
        return builder;
    }
}
