using Coaching.Infrastructure;
using Xunit;

namespace Coaching.Tests;

public sealed partial class DomainTests
{
    [Theory]
    [InlineData("2026-01-01T21:59:59Z", "2026-01-01")]
    [InlineData("2026-01-01T22:00:00Z", "2026-01-02")]
    [InlineData("2026-07-01T20:59:59Z", "2026-07-01")]
    [InlineData("2026-07-01T21:00:00Z", "2026-07-02")]
    public void Workout_dates_use_Cairo_midnight_and_daylight_saving(string instant, string date)
    {
        var time = new CairoTime(TimeProvider.System, TimeZoneInfo.FindSystemTimeZoneById("Africa/Cairo"));
        Assert.Equal(DateOnly.Parse(date), time.LocalDate(DateTimeOffset.Parse(instant)));
    }
}
