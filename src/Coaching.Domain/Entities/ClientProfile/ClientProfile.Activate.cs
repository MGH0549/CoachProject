namespace Coaching.Domain;

public sealed partial class ClientProfile
{
    public string? Activate(string package, DateTimeOffset end, DateTimeOffset now)
    {
        if (end <= now)
            return "access_end_in_past";
        if (string.IsNullOrWhiteSpace(package))
            return "package_required";
        Package = package;
        AccessEndsAt = end;
        Status = ClientStatus.Active;
        ActivatedAt ??= now;
        DeactivatedAt = null;
        return null;
    }
}
