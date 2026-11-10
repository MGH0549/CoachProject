
namespace Coaching.Application;

public partial interface IAccounts
{
    Task<Result<string>> Register(string email, string password, string name, string whatsapp, CancellationToken ct);
}
