
namespace Coaching.Infrastructure.Persistence;

public sealed partial class CoachingDbContext
{
    public async Task Save(CancellationToken ct) => await SaveChangesAsync(ct);
}
