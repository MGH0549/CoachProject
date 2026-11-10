
namespace Coaching.Application.Auth;

public sealed class ResetPasswordHandler(IAccounts accounts)
{
    public Task<Result<Done>> Handle(ResetPasswordRequest request, CancellationToken ct) => accounts.ResetPassword(request.Email, request.Token, request.Password, ct);
}
