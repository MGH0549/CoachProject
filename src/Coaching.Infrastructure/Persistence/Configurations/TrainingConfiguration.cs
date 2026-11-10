using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Coaching.Domain;

namespace Coaching.Infrastructure.Persistence;

public sealed class TrainingConfiguration : IEntityTypeConfiguration<TrainingPlan>
{
    public void Configure(EntityTypeBuilder<TrainingPlan> b)
    {
        b.HasIndex(x => x.ClientId).IsUnique().HasFilter("[ClientId] IS NOT NULL");
        b.ToTable(t => t.HasCheckConstraint("CK_Training_Owner", "([IsTemplate] = 1 AND [ClientId] IS NULL) OR ([IsTemplate] = 0 AND [ClientId] IS NOT NULL)"));
        b.HasMany(x => x.Days).WithOne().HasForeignKey(x => x.TrainingPlanId).OnDelete(DeleteBehavior.Cascade);
    }
}
