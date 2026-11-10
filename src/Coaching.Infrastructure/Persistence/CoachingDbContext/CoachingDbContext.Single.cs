using Microsoft.EntityFrameworkCore;

namespace Coaching.Infrastructure.Persistence;

public sealed partial class CoachingDbContext
{
    public Task<T?> Single<T>(IQueryable<T> query, CancellationToken ct) => query.SingleOrDefaultAsync(ct);
}
