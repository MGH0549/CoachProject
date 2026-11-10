using Coaching.Domain;

namespace Coaching.Application;

public partial interface IStore
{
    IQueryable<DietPlan> Diet(bool tracking = false);
}
