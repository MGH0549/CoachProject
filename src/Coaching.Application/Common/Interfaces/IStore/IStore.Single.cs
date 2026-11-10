
namespace Coaching.Application;

public partial interface IStore
{
    Task<T?> Single<T>(IQueryable<T> query, CancellationToken ct);
}
