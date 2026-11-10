
namespace Coaching.Application.Progress;

public record WorkoutResponse(Guid Id, Guid? PlanExerciseId, Guid MainExerciseId, Guid PerformedExerciseId, DateTimeOffset CompletedAt, DateOnly CairoDate, int Sets, string Reps);
