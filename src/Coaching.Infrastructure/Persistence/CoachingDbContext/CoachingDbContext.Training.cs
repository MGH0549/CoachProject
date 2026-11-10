using Microsoft.EntityFrameworkCore;
using Coaching.Domain;

namespace Coaching.Infrastructure.Persistence;

public sealed partial class CoachingDbContext
{
    public IQueryable<TrainingPlan> Training(bool tracking = false) => Query<TrainingPlan>(tracking).Include(x => x.Days).ThenInclude(x => x.Exercises).ThenInclude(x => x.Exercise).Include(x => x.Days).ThenInclude(x => x.Exercises).ThenInclude(x => x.Alternatives).ThenInclude(x => x.Exercise).AsSingleQuery();
}
