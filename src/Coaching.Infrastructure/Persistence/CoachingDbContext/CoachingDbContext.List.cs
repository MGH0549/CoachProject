using Microsoft.EntityFrameworkCore;

namespace Coaching.Infrastructure.Persistence;

public sealed partial class CoachingDbContext
{
    public Task<List<T>> List<T>(IQueryable<T> query, CancellationToken ct) => query.ToListAsync(ct);
}
