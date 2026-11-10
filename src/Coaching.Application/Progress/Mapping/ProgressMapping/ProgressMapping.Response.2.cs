using Coaching.Domain;

namespace Coaching.Application.Progress;

public static partial class ProgressMapping
{
    public static WorkoutResponse Response(this WorkoutLog x) => new(x.Id, x.PlanExerciseId, x.MainExerciseId, x.PerformedExerciseId, x.CompletedAt, x.CairoDate, x.Sets, x.Reps);
}
