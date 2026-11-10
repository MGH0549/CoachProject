using Coaching.Domain;

namespace Coaching.Application;

public partial interface IStore
{
    void Add<T>(T entity)
        where T : Entity;
}
