namespace Coaching.Domain;

public sealed class EmailMessage : Entity
{
    public DateTimeOffset? ReminderForEnd { get; set; }
    public string Recipient { get; set; } = "";
    public string Subject { get; set; } = "";
    public string Html { get; set; } = "";
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset NextAttemptAt { get; set; }
    public int Attempts { get; set; }
    public string? ClientId { get; set; }
}
