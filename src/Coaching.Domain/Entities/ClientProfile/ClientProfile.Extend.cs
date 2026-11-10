namespace Coaching.Domain;

public sealed partial class ClientProfile
{
    public string? Extend(DateTimeOffset end, DateTimeOffset now)
    {
        if (Status is ClientStatus.Pending or ClientStatus.Deactivated)
            return "activation_required";
        if (end <= now || end <= AccessEndsAt)
            return "extension_must_increase";
        AccessEndsAt = end;
        Status = ClientStatus.Active;
        return null;
    }
}
