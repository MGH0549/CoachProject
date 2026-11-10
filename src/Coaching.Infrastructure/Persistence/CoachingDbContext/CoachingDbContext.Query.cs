using Microsoft.EntityFrameworkCore;
using Coaching.Domain;

namespace Coaching.Infrastructure.Persistence;

public sealed partial class CoachingDbContext
{
    public IQueryable<T> Query<T>(bool tracking = false)
        where T : Entity => tracking ? Set<T>() : Set<T>().AsNoTracking();
}
