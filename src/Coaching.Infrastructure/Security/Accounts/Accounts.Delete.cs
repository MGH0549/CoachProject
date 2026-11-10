using Microsoft.EntityFrameworkCore;
using System.Data;
using Coaching.Domain;

namespace Coaching.Infrastructure.Security;

public sealed partial class Accounts
{
    public async Task Delete(string userId, CancellationToken ct)
    {
        await db.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
        {
            db.ChangeTracker.Clear();
            await using var tx = await db.Database.BeginTransactionAsync(ct);
            // Parent cascades remove plan children, selections, check-in photos, and Identity credentials.
            await db.Set<WorkoutLog>().Where(x => x.ClientId == userId).ExecuteDeleteAsync(ct);
            await db.Set<TrainingPlan>().Where(x => x.ClientId == userId).ExecuteDeleteAsync(ct);
            await db.Set<DietPlan>().Where(x => x.ClientId == userId).ExecuteDeleteAsync(ct);
            await db.Set<CheckIn>().Where(x => x.ClientId == userId).ExecuteDeleteAsync(ct);
            await db.Set<EmailMessage>().Where(x => x.ClientId == userId).ExecuteDeleteAsync(ct);
            await db.Set<AuditEntry>().Where(x => x.ActorId == userId || x.SubjectClientId == userId || x.RecordId == userId).ExecuteDeleteAsync(ct);
            var user = await users.FindByIdAsync(userId);
            if (user is not null && !(await users.DeleteAsync(user)).Succeeded)
                throw new InvalidOperationException("Account deletion failed.");
            // Keep a non-identifying deletion event only.
            db.Add(new AuditEntry { ActorId = "system", Action = "client.deleted", RecordId = "erased", At = time.UtcNow });
            await db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
        });
    }
}
