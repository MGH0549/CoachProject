using Microsoft.AspNetCore.Mvc.Testing;

namespace Coaching.Tests;

public sealed partial class TestApiFactory : WebApplicationFactory<Program>
{
    public const string Password = "Test-Password-812!";
    public const string CoachEmail = "coach@example.test";
    private readonly string databaseName = "CoachingTests_" + Guid.NewGuid().ToString("N");
    public TestPhotos Photos { get; } = new();
    public TestEmailTransport Emails { get; } = new();
    public TestClock Clock { get; } = new();

    private bool initialized;
}
