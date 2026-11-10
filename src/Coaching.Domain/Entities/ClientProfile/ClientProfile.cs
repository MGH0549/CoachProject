namespace Coaching.Domain;

public sealed partial class ClientProfile : ClientEntity
{
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string WhatsAppNumber { get; set; } = "";
    public string? Package { get; set; }
    public ClientStatus Status { get; set; } = ClientStatus.Pending;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ConsentAt { get; set; }
    public DateTimeOffset? ActivatedAt { get; set; }
    public DateTimeOffset? AccessEndsAt { get; set; }
    public DateTimeOffset? DeactivatedAt { get; set; }
    public DateTimeOffset? ReminderSentForEnd { get; set; }
}
