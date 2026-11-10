using Microsoft.EntityFrameworkCore;
using Coaching.Domain;

namespace Coaching.Infrastructure.Persistence;

public sealed partial class CoachingDbContext
{
    public IQueryable<CheckIn> CheckIns(bool tracking = false) => Query<CheckIn>(tracking).Include(x => x.Photos);
}
