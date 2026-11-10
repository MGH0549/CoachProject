namespace Coaching.Domain;

public sealed partial class ClientProfile
{
    public DateTimeOffset? AccessEndedAt(DateTimeOffset now)
    {
        if (Status == ClientStatus.Deactivated)
            return AccessEndsAt is { } end && end < DeactivatedAt ? end : DeactivatedAt;
        return AccessEndsAt <= now ? AccessEndsAt : null;
    }
}
