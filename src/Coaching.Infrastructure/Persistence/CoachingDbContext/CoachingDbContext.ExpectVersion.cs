using Coaching.Domain;

namespace Coaching.Infrastructure.Persistence;

public sealed partial class CoachingDbContext
{
    public void ExpectVersion(Entity entity, byte[] version)
    {
        Entry(entity).Property(x => x.RowVersion).OriginalValue = version;
        // Force an UPDATE even when the submitted values are unchanged; this checks the version and advances it.
        var property = Entry(entity).Properties.First(p => !p.Metadata.IsPrimaryKey() && !p.Metadata.IsConcurrencyToken && !p.Metadata.IsForeignKey());
        property.IsModified = true;
    }
}
