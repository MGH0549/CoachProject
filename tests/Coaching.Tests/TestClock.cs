namespace Coaching.Tests;

public sealed class TestClock : TimeProvider
{
    public DateTimeOffset Current { get; set; } = DateTimeOffset.UtcNow;
    public override DateTimeOffset GetUtcNow() => Current;
}
