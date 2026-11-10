using Coaching.Application;

namespace Coaching.Infrastructure.Security;

public sealed partial class Accounts
{
    public async Task<Result<Session>> Login(string email, string password, CancellationToken ct)
    {
        var user = await users.FindByEmailAsync(email);
        if (user is null || await users.IsLockedOutAsync(user))
            return InvalidSession();
        if (!await users.CheckPasswordAsync(user, password))
        {
            await users.AccessFailedAsync(user);
            return InvalidSession();
        }

        await users.ResetAccessFailedCountAsync(user);
        var session = await Issue(user, Guid.NewGuid(), time.UtcNow.AddDays(jwt.Value.RefreshDays));
        await db.SaveChangesAsync(ct);
        return Result<Session>.Ok(session);
    }
}
