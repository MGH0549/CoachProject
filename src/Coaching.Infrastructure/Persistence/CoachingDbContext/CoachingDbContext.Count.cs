using Microsoft.EntityFrameworkCore;

namespace Coaching.Infrastructure.Persistence;

public sealed partial class CoachingDbContext
{
    public Task<int> Count<T>(IQueryable<T> query, CancellationToken ct) => query.CountAsync(ct);
}
