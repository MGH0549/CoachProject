using Coaching.Application;

namespace Coaching.Tests;

public sealed partial class IntegrationTests
{
    private sealed class TestActor(string? id) : IActor
    {
        public string? Id { get; set; } = id;
        public string? UserId => Id;
        public bool IsCoach => false;
    }
}
