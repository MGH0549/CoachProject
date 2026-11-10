using Microsoft.EntityFrameworkCore;
using System.Data;
using Coaching.Domain;

namespace Coaching.Infrastructure.Security;

public sealed partial class Accounts
{
    public async Task Logout(string token, CancellationToken ct)
    {
        if (token.Length is < 40 or > 200)
            return;
        var hash = Hash(token);
        var family = await db.Set<RefreshToken>().Where(x => x.Hash == hash).Select(x => (Guid?)x.Family).SingleOrDefaultAsync(ct);
        if (family is not null)
            await db.Set<RefreshToken>().Where(x => x.Family == family && x.RevokedAt == null).ExecuteUpdateAsync(s => s.SetProperty(x => x.RevokedAt, time.UtcNow), ct);
    }
}
