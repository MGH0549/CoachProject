
namespace Coaching.Application;

public partial interface IStore
{
    Task<List<T>> List<T>(IQueryable<T> query, CancellationToken ct);
}
