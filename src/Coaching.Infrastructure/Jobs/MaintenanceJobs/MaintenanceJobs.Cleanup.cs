using Microsoft.EntityFrameworkCore;
using System.Data;
using Coaching.Domain;

namespace Coaching.Infrastructure.Jobs;

public sealed partial class MaintenanceJobs
{
    public async Task Cleanup(CancellationToken ct)
    {
        var cutoff = time.UtcNow.AddDays(-platform.Value.PhotoRetentionDays);
        var ids = await db.Set<ClientProfile>().AsNoTracking().Where(x => x.DeactivatedAt < cutoff || x.AccessEndsAt < cutoff).Select(x => x.Id).ToListAsync(ct);
        foreach (var id in ids)
        {
            await db.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
            {
                await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);
                var p = await db.Set<ClientProfile>().FromSqlInterpolated($"SELECT * FROM [ClientProfile] WITH (UPDLOCK, ROWLOCK) WHERE [Id] = {id}").SingleOrDefaultAsync(ct);
                if (p is null || p.HasAccess(time.UtcNow) || p.AccessEndedAt(time.UtcNow) is not { } ended || ended >= cutoff)
                    return;
                var files = await db.Set<ProgressPhoto>().Where(x => x.ClientId == p.ClientId).ToListAsync(ct);
                foreach (var file in files)
                {
                    await photos.Delete(file.FileReference, ct);
                    db.Remove(file);
                }

                await db.SaveChangesAsync(ct);
                await tx.CommitAsync(ct);
            });
        }

        await db.Set<RefreshToken>().Where(x => x.ExpiresAt < time.UtcNow).ExecuteDeleteAsync(ct);
        var auditCutoff = time.UtcNow.AddDays(-platform.Value.AuditRetentionDays);
        await db.Set<AuditEntry>().Where(x => x.At < auditCutoff).ExecuteDeleteAsync(ct);
        var emailCutoff = time.UtcNow.AddDays(-1);
        await db.Set<EmailMessage>().Where(x => x.CreatedAt < emailCutoff).ExecuteDeleteAsync(ct);
    }
}
