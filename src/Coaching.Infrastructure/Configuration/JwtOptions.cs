
namespace Coaching.Infrastructure.Security;

public sealed class JwtOptions
{
    public string Issuer { get; set; } = "Coaching.Api";
    public string Audience { get; set; } = "Coaching.Angular";
    public string Key { get; set; } = "";
    public int AccessMinutes { get; set; } = 15;
    public int RefreshDays { get; set; } = 30;
}
