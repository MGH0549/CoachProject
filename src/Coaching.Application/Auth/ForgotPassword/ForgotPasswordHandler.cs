
namespace Coaching.Application.Auth;

public sealed class ForgotPasswordHandler(IAccounts accounts)
{
    public Task Handle(ForgotPasswordRequest request, CancellationToken ct) => accounts.ForgotPassword(request.Email, ct);
}
