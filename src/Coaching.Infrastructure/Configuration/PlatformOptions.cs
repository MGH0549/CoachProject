
namespace Coaching.Infrastructure.Security;

public sealed class PlatformOptions
{
    public string TimeZone { get; set; } = "Africa/Cairo";
    public string AngularOrigin { get; set; } = "";
    public string CoachWhatsApp { get; set; } = "";
    public string PrivacyPolicyUrl { get; set; } = "";
    public int ReminderLeadDays { get; set; } = 3;
    public int PhotoRetentionDays { get; set; } = 7;
    public int AuditRetentionDays { get; set; } = 365;
    public DayOfWeek CheckInDueDay { get; set; } = DayOfWeek.Sunday;
}
