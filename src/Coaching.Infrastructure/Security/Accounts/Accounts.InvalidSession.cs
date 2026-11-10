using Coaching.Application;

namespace Coaching.Infrastructure.Security;

public sealed partial class Accounts
{
    private static Result<Session> InvalidSession() => Result<Session>.Fail("invalid_credentials", "بيانات الدخول أو الجلسة غير صالحة", 401);
}
