using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Coaching.Domain;

namespace Coaching.Infrastructure.Persistence;

public sealed class AlternativeConfiguration : IEntityTypeConfiguration<ExerciseAlternative>
{
    public void Configure(EntityTypeBuilder<ExerciseAlternative> b) => b.HasOne(x => x.Exercise).WithMany().HasForeignKey(x => x.ExerciseId).OnDelete(DeleteBehavior.Restrict);
}
