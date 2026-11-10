
namespace Coaching.Application.Auth;

public sealed class LoginHandler(IAccounts accounts)
{
    public Task<Result<Session>> Handle(LoginRequest request, CancellationToken ct) => accounts.Login(request.Email, request.Password, ct);
}
