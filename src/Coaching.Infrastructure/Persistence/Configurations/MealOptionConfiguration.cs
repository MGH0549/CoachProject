using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Coaching.Domain;

namespace Coaching.Infrastructure.Persistence;

public sealed class MealOptionConfiguration : IEntityTypeConfiguration<MealOption>
{
    public void Configure(EntityTypeBuilder<MealOption> b)
    {
        b.HasMany(x => x.Items).WithOne().HasForeignKey(x => x.MealOptionId).OnDelete(DeleteBehavior.Cascade);
        b.ToTable(t => t.HasCheckConstraint("CK_Option_Macros", "[Calories] >= 0 AND [Protein] >= 0 AND [Carbs] >= 0 AND [Fat] >= 0"));
    }
}
