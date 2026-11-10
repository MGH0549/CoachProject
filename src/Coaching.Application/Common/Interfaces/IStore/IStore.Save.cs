
namespace Coaching.Application;

public partial interface IStore
{
    Task Save(CancellationToken ct);
}
