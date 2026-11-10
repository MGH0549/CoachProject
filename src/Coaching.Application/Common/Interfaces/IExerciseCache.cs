
namespace Coaching.Application;

public interface IExerciseCache
{
    long Version { get; }

    void Invalidate();
}
