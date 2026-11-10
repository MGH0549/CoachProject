
namespace Coaching.Application;

public interface IActor
{
    string? UserId { get; }

    bool IsCoach { get; }
}
