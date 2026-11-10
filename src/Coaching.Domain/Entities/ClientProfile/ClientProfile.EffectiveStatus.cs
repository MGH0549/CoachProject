namespace Coaching.Domain;

public sealed partial class ClientProfile
{
    public ClientStatus EffectiveStatus(DateTimeOffset now) => Status == ClientStatus.Active && AccessEndsAt <= now ? ClientStatus.Expired : Status;
}
