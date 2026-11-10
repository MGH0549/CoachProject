
namespace Coaching.Application;

public partial interface IAccounts
{
    Task Delete(string userId, CancellationToken ct);
}
