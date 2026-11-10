using Coaching.Domain;

namespace Coaching.Infrastructure.Persistence;

public sealed partial class CoachingDbContext
{
    public void Audit(string action, string recordId) => Set<AuditEntry>().Add(new() { ActorId = actor.UserId ?? "system", Action = action, RecordId = recordId, At = time.UtcNow, SubjectClientId = ChangeTracker.Entries<ClientEntity>().FirstOrDefault(x => x.Entity.Id.ToString() == recordId)?.Entity.ClientId ?? (Guid.TryParse(recordId, out _) ? null : recordId) });
}
