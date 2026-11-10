using Coaching.Application;

namespace Coaching.Api;

public sealed partial class DesignTimeDbFactory
{
    private sealed class DesignActor : IActor
    {
        public string? UserId => null;
        public bool IsCoach => false;
    }
}
