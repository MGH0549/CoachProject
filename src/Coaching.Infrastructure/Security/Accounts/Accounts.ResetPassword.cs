using Microsoft.EntityFrameworkCore;
using System.Data;
using Coaching.Application;
using Coaching.Domain;

namespace Coaching.Infrastructure.Security;

public sealed partial class Accounts
{
    public async Task<Result<Done>> ResetPassword(string email, string token, string password, CancellationToken ct)
    {
        var user = await users.FindByEmailAsync(email);
        if (user is null)
            return Result<Done>.Fail("invalid_reset", "الرابط غير صالح أو انتهت صلاحيته");
        var result = await users.ResetPasswordAsync(user, token, password);
        if (!result.Succeeded)
            return Result<Done>.Fail("invalid_reset", "الرابط غير صالح أو كلمة المرور لا تستوفي الشروط");
        await db.Set<RefreshToken>().Where(x => x.UserId == user.Id && x.RevokedAt == null).ExecuteUpdateAsync(s => s.SetProperty(x => x.RevokedAt, time.UtcNow), ct);
        return Result<Done>.Ok(new());
    }
}
