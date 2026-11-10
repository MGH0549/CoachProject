
namespace Coaching.Application.Auth;

public sealed class RegisterHandler(IAccounts accounts)
{
    public Task<Result<string>> Handle(RegisterRequest request, CancellationToken ct) =>
     accounts.Register(request.Email, request.Password, request.FullName, request.WhatsAppNumber, ct);
}
