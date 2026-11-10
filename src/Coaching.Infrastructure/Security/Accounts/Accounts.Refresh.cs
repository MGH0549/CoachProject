using Microsoft.EntityFrameworkCore;
using System.Data;
using Coaching.Application;
using Coaching.Domain;

namespace Coaching.Infrastructure.Security;

public sealed partial class Accounts
{
    public async Task<Result<Session>> Refresh(string token, CancellationToken ct)
    {
        if (token.Length is < 40 or > 200)
            return InvalidSession();
        var hash = Hash(token);
        return await db.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
        {
            db.ChangeTracker.Clear();
            await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);
            // Lock the presented row before reading it. A second refresh sees the first refresh's revocation.
            var old = await db.Set<RefreshToken>().FromSqlInterpolated($"SELECT * FROM [RefreshToken] WITH (UPDLOCK, ROWLOCK) WHERE [Hash] = {hash}").SingleOrDefaultAsync(ct);
            if (old is null || old.ExpiresAt <= time.UtcNow)
                return InvalidSession();
            if (old.RevokedAt is not null)
            {
                await db.Set<RefreshToken>().Where(x => x.Family == old.Family && x.RevokedAt == null).ExecuteUpdateAsync(s => s.SetProperty(x => x.RevokedAt, time.UtcNow), ct);
                await tx.CommitAsync(ct);
                return Result<Session>.Fail("refresh_reuse", "انتهت الجلسة. يرجى تسجيل الدخول مجدداً", 401);
            }

            var user = await users.FindByIdAsync(old.UserId);
            if (user is null || old.SecurityStamp != user.SecurityStamp || await users.IsLockedOutAsync(user))
                return InvalidSession();
            old.RevokedAt = time.UtcNow;
            var session = await Issue(user, old.Family, old.ExpiresAt);
            await db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
            return Result<Session>.Ok(session);
        });
    }
}
