using System.Security.Claims;
using Coaching.Application;
using Coaching.Domain;
using Microsoft.AspNetCore.Http;

namespace Coaching.Infrastructure.Security;

public sealed class RequestActor(IHttpContextAccessor context) : IActor
{
    // Set only inside a fresh background-service scope or explicit administrative command, never from request input.
    public bool IsSystem { get; set; }
    public string? UserId => context.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    public bool IsCoach => IsSystem || context.HttpContext?.User.IsInRole(Roles.Coach) == true;
}
