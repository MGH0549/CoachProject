namespace Coaching.Domain;

public sealed partial class ClientProfile
{
    public bool HasAccess(DateTimeOffset now) => Status == ClientStatus.Active && AccessEndsAt > now;
}
