using Coaching.Application;

namespace Coaching.Infrastructure.Persistence;

public sealed partial class CoachingDbContext
{
    void IStore.Remove<T>(T entity) => Set<T>().Remove(entity);
}
