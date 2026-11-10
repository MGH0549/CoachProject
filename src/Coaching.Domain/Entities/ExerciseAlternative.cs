namespace Coaching.Domain;

public sealed class ExerciseAlternative : ClientEntity
{
    public Guid PlanExerciseId { get; set; }
    public Guid ExerciseId { get; set; }
    public Exercise Exercise { get; set; } = null!;
    public int? Sets { get; set; }
    public string? Reps { get; set; }
    public string? Note { get; set; }
}
