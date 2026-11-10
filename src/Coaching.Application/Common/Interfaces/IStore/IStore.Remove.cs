using Coaching.Domain;

namespace Coaching.Application;

public partial interface IStore
{
    void Remove<T>(T entity)
        where T : Entity;
}
