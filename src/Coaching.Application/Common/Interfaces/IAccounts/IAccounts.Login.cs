
namespace Coaching.Application;

public partial interface IAccounts
{
    Task<Result<Session>> Login(string email, string password, CancellationToken ct);
}
