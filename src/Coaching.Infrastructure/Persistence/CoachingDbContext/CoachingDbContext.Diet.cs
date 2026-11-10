using Microsoft.EntityFrameworkCore;
using Coaching.Domain;

namespace Coaching.Infrastructure.Persistence;

public sealed partial class CoachingDbContext
{
    public IQueryable<DietPlan> Diet(bool tracking = false) => Query<DietPlan>(tracking).Include(x => x.Meals).ThenInclude(x => x.Options).ThenInclude(x => x.Items).Include(x => x.Meals).ThenInclude(x => x.Selection).AsSingleQuery();
}
