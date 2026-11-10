using Coaching.Application;

namespace Coaching.Infrastructure.Persistence;

public sealed partial class CoachingDbContext
{
    void IStore.Add<T>(T entity) => Set<T>().Add(entity);
}
