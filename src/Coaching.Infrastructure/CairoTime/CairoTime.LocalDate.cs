
namespace Coaching.Infrastructure;

public sealed partial class CairoTime
{
    public DateOnly LocalDate(DateTimeOffset utc) => DateOnly.FromDateTime(TimeZoneInfo.ConvertTime(utc, zone).DateTime);
}
