namespace Coaching.Domain;

public sealed class WorkoutLog : ClientEntity
{
    public Guid? PlanExerciseId { get; set; }
    public Guid MainExerciseId { get; set; }
    public Guid PerformedExerciseId { get; set; }
    public DateTimeOffset CompletedAt { get; set; }
    public DateOnly CairoDate { get; set; }
    public int Sets { get; set; }
    public string Reps { get; set; } = "";
}
