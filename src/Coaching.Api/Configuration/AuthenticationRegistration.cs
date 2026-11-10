using System.Security.Claims;
using System.Text;
using Coaching.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Coaching.Api;

public static class AuthenticationRegistration
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
        {
            var jwt = configuration.GetSection("Jwt").Get<JwtOptions>() ?? new();
            o.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = jwt.Issuer,
                ValidAudience = jwt.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
                ClockSkew = TimeSpan.Zero,
                ValidAlgorithms = [SecurityAlgorithms.HmacSha256]
            };
            o.Events = new JwtBearerEvents
            {
                OnTokenValidated = async c =>
            {
                var id = c.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
                var manager = c.HttpContext.RequestServices.GetRequiredService<UserManager<AppUser>>();
                var user = id is null ? null : await manager.FindByIdAsync(id);
                if (user is null || user.SecurityStamp != c.Principal?.FindFirstValue("stamp")) c.Fail("Invalid session.");
            }
            };
        });
        return services;
    }
}
