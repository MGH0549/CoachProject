using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Coaching.Domain;

namespace Coaching.Infrastructure.Persistence;

public sealed class MealConfiguration : IEntityTypeConfiguration<Meal>
{
    public void Configure(EntityTypeBuilder<Meal> b)
    {
        b.HasIndex(x => new { x.DietPlanId, x.Order });
        b.HasMany(x => x.Options).WithOne().HasForeignKey(x => x.MealId).OnDelete(DeleteBehavior.Cascade);
        b.HasOne(x => x.Selection).WithOne().HasForeignKey<ClientMealSelection>(x => x.MealId).OnDelete(DeleteBehavior.Cascade);
    }
}
