
namespace Coaching.Application;

public partial interface IAccounts
{
    Task ForgotPassword(string email, CancellationToken ct);
}
