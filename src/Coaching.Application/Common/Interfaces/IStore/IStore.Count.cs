
namespace Coaching.Application;

public partial interface IStore
{
    Task<int> Count<T>(IQueryable<T> query, CancellationToken ct);
}
