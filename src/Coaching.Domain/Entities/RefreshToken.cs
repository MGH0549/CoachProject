namespace Coaching.Domain;

public sealed class RefreshToken : Entity
{
    public string UserId { get; set; } = "";
    public Guid Family { get; set; }
    public string Hash { get; set; } = "";
    public string SecurityStamp { get; set; } = "";
    public DateTimeOffset ExpiresAt { get; set; }
    public DateTimeOffset? RevokedAt { get; set; }
}
