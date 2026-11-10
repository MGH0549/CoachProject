using System.Threading.RateLimiting;

namespace Coaching.Api;

public static class RateLimitRegistration
{
    public static IServiceCollection AddRequestRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(o =>
        {
            o.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            o.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(c => RateLimitPartition.GetFixedWindowLimiter(
                c.Connection.RemoteIpAddress?.ToString() ?? "unknown", _ => new() { PermitLimit = 300, Window = TimeSpan.FromMinutes(1), QueueLimit = 0 }));
            o.AddPolicy("auth", c => RateLimitPartition.GetFixedWindowLimiter(c.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                _ => new() { PermitLimit = 20, Window = TimeSpan.FromMinutes(1), QueueLimit = 0 }));
        });
        return services;
    }
}
