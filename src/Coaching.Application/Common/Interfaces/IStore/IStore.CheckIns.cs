using Coaching.Domain;

namespace Coaching.Application;

public partial interface IStore
{
    IQueryable<CheckIn> CheckIns(bool tracking = false);
}
