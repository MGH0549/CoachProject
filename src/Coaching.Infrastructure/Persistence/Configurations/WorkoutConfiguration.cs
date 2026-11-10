using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Coaching.Domain;

namespace Coaching.Infrastructure.Persistence;

public sealed class WorkoutConfiguration : IEntityTypeConfiguration<WorkoutLog>
{
    public void Configure(EntityTypeBuilder<WorkoutLog> b)
    {
        b.HasIndex(x => new { x.ClientId, x.CairoDate });
        b.HasOne<PlanExercise>().WithMany().HasForeignKey(x => x.PlanExerciseId).OnDelete(DeleteBehavior.SetNull);
        b.HasOne<Exercise>().WithMany().HasForeignKey(x => x.MainExerciseId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne<Exercise>().WithMany().HasForeignKey(x => x.PerformedExerciseId).OnDelete(DeleteBehavior.Restrict);
    }
}
