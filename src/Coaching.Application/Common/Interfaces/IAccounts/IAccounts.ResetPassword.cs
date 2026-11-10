
namespace Coaching.Application;

public partial interface IAccounts
{
    Task<Result<Done>> ResetPassword(string email, string token, string password, CancellationToken ct);
}
