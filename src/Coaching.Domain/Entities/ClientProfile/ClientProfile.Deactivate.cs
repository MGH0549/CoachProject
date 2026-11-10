namespace Coaching.Domain;

public sealed partial class ClientProfile
{
    public void Deactivate(DateTimeOffset now)
    {
        if (Status != ClientStatus.Deactivated)
            DeactivatedAt = now;
        Status = ClientStatus.Deactivated;
    }
}
