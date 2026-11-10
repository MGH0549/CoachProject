
namespace Coaching.Application;

public partial interface IAccounts
{
    Task<Result<Session>> Refresh(string token, CancellationToken ct);
}
