using Coaching.Application;

namespace Coaching.Infrastructure;

public sealed partial class CairoTime(TimeProvider clock, TimeZoneInfo zone) : ICairoTime
{
    public DateTimeOffset UtcNow => clock.GetUtcNow();
    public DateOnly Today => LocalDate(UtcNow);
}
