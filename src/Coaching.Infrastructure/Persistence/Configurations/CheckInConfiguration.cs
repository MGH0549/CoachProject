using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Coaching.Domain;

namespace Coaching.Infrastructure.Persistence;

public sealed class CheckInConfiguration : IEntityTypeConfiguration<CheckIn>
{
    public void Configure(EntityTypeBuilder<CheckIn> b)
    {
        b.HasIndex(x => new { x.ClientId, x.CairoDate });
        b.HasMany(x => x.Photos).WithOne().HasForeignKey(x => x.CheckInId).OnDelete(DeleteBehavior.Cascade);
        b.ToTable(t => t.HasCheckConstraint("CK_CheckIn_Range", "[Weight] > 0 AND [Energy] BETWEEN 1 AND 10"));
    }
}
