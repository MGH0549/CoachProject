using Coaching.Infrastructure;
using Xunit;

namespace Coaching.Tests;

public sealed partial class DomainTests
{
    [Theory]
    [InlineData("2026-04-23")]
    [InlineData("2026-04-24")]
    [InlineData("2026-10-29")]
    [InlineData("2026-10-30")]
    public void End_of_day_handles_both_daylight_saving_transitions(string date)
    {
        var time = new CairoTime(TimeProvider.System, TimeZoneInfo.FindSystemTimeZoneById("Africa/Cairo"));
        var day = DateOnly.Parse(date);
        var end = time.EndOfDay(day);
        Assert.Equal(TimeSpan.Zero, end.Offset);
        Assert.Equal(day, time.LocalDate(end));
        Assert.Equal(day.AddDays(1), time.LocalDate(end.AddTicks(1)));
    }
}
