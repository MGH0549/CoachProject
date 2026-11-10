using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Coaching.Domain;

namespace Coaching.Infrastructure.Persistence;

public sealed class DietConfiguration : IEntityTypeConfiguration<DietPlan>
{
    public void Configure(EntityTypeBuilder<DietPlan> b)
    {
        b.HasIndex(x => x.ClientId).IsUnique().HasFilter("[ClientId] IS NOT NULL");
        b.ToTable(t => t.HasCheckConstraint("CK_Diet_Owner", "([IsTemplate] = 1 AND [ClientId] IS NULL) OR ([IsTemplate] = 0 AND [ClientId] IS NOT NULL)"));
        b.HasMany(x => x.Meals).WithOne().HasForeignKey(x => x.DietPlanId).OnDelete(DeleteBehavior.Cascade);
    }
}
