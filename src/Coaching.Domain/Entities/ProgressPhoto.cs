namespace Coaching.Domain;

public sealed class ProgressPhoto : ClientEntity
{
    public Guid CheckInId { get; set; }
    public string FileReference { get; set; } = "";
    public DateTimeOffset CreatedAt { get; set; }
}
