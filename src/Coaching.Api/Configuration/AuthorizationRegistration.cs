using System.Security.Claims;
using Coaching.Application;
using Coaching.Domain;
using Coaching.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Coaching.Api;

public static class AuthorizationRegistration
{
    public static IServiceCollection AddAccountAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(o =>
        {
            o.AddPolicy("Coach", p => p.RequireRole(Roles.Coach));
            o.AddPolicy("Client", p => p.RequireRole(Roles.Client));
            o.AddPolicy("ActiveClient", p => p.RequireRole(Roles.Client).RequireAssertion(async c =>
            {
                if (c.Resource is not HttpContext http) return false;
                var id = http.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var db = http.RequestServices.GetRequiredService<CoachingDbContext>();
                var now = http.RequestServices.GetRequiredService<ICairoTime>().UtcNow;
                return await db.Set<ClientProfile>().AsNoTracking().AnyAsync(x => x.ClientId == id && x.Status == ClientStatus.Active && x.AccessEndsAt > now, http.RequestAborted);
            }));
        });
        return services;
    }
}
