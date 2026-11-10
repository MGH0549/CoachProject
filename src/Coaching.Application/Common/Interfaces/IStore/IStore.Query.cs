using Coaching.Domain;

namespace Coaching.Application;

public partial interface IStore
{
    IQueryable<T> Query<T>(bool tracking = false)
        where T : Entity;
}
