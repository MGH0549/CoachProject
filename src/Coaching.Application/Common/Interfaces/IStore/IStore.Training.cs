using Coaching.Domain;

namespace Coaching.Application;

public partial interface IStore
{
    IQueryable<TrainingPlan> Training(bool tracking = false);
}
