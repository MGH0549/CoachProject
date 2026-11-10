
namespace Coaching.Application.Auth;

public record ResetPasswordRequest(string Email, string Token, string Password);
