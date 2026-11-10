using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Coaching.Domain;

namespace Coaching.Infrastructure.Persistence;

public sealed class DayConfiguration : IEntityTypeConfiguration<PlanDay>
{
    public void Configure(EntityTypeBuilder<PlanDay> b)
    {
        b.HasIndex(x => new { x.TrainingPlanId, x.Order });
        b.HasMany(x => x.Exercises).WithOne().HasForeignKey(x => x.PlanDayId).OnDelete(DeleteBehavior.Cascade);
    }
}
