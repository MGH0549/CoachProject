
namespace Coaching.Application;

public partial interface IAccounts
{
    Task Logout(string token, CancellationToken ct);
}
