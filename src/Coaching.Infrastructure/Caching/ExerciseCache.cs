using Coaching.Application;

namespace Coaching.Infrastructure;

public sealed class ExerciseCache : IExerciseCache
{
    private long version;
    public long Version => Interlocked.Read(ref version);

    public void Invalidate() => Interlocked.Increment(ref version);
}
