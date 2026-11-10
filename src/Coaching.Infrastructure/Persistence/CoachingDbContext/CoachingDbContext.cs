using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Coaching.Application;
using Coaching.Infrastructure.Security;

namespace Coaching.Infrastructure.Persistence;

public sealed partial class CoachingDbContext(DbContextOptions<CoachingDbContext> options, IActor actor, ICairoTime time) : IdentityDbContext<AppUser>(options), IStore
{
    // These are instance members: EF parameterizes them for each request, never the first user in its model cache.
    public string? CurrentUserId => actor.UserId;
    public bool CanSeeAll => actor.IsCoach;
}
