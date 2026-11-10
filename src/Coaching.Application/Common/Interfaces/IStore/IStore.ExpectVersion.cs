using Coaching.Domain;

namespace Coaching.Application;

public partial interface IStore
{
    void ExpectVersion(Entity entity, byte[] version);
}
