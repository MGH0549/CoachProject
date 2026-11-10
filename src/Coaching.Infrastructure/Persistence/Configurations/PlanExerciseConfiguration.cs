using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Coaching.Domain;

namespace Coaching.Infrastructure.Persistence;

public sealed class PlanExerciseConfiguration : IEntityTypeConfiguration<PlanExercise>
{
    public void Configure(EntityTypeBuilder<PlanExercise> b)
    {
        b.HasIndex(x => new { x.PlanDayId, x.Order });
        b.HasOne(x => x.Exercise).WithMany().HasForeignKey(x => x.ExerciseId).OnDelete(DeleteBehavior.Restrict);
        b.HasMany(x => x.Alternatives).WithOne().HasForeignKey(x => x.PlanExerciseId).OnDelete(DeleteBehavior.Cascade);
        b.ToTable(t => t.HasCheckConstraint("CK_PlanExercise_Sets", "[Sets] > 0 AND [RestSeconds] >= 0"));
    }
}
