namespace Coaching.Domain;

public sealed class AuditEntry : Entity
{
    public string? SubjectClientId { get; set; }
    public string ActorId { get; set; } = "";
    public string Action { get; set; } = "";
    public string RecordId { get; set; } = "";
    public DateTimeOffset At { get; set; }
}
