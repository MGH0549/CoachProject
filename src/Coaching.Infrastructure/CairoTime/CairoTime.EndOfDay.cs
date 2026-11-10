
namespace Coaching.Infrastructure;

public sealed partial class CairoTime
{
    public DateTimeOffset EndOfDay(DateOnly date)
    {
        // The next local day may begin in a DST gap. Locate its first valid instant and use an exclusive boundary minus one tick.
        var next = date.AddDays(1).ToDateTime(TimeOnly.MinValue, DateTimeKind.Unspecified);
        while (zone.IsInvalidTime(next))
            next = next.AddMinutes(1);
        if (zone.IsAmbiguousTime(next))
        {
            var earlierUtc = next - zone.GetAmbiguousTimeOffsets(next).Max();
            return new DateTimeOffset(DateTime.SpecifyKind(earlierUtc, DateTimeKind.Utc)).AddTicks(-1);
        }

        return new DateTimeOffset(TimeZoneInfo.ConvertTimeToUtc(next, zone)).AddTicks(-1);
    }
}
