namespace Coaching.Domain;

public sealed class PlanExercise : ClientEntity
{
    public Guid PlanDayId { get; set; }
    public Guid ExerciseId { get; set; }
    public Exercise Exercise { get; set; } = null!;
    public int Sets { get; set; }
    public string Reps { get; set; } = "";
    public int RestSeconds { get; set; }
    public string? Tempo { get; set; }
    public string? Note { get; set; }
    public int Order { get; set; }
    public bool ChoiceLocked { get; set; }
    public Guid? SelectedAlternativeId { get; set; }
    public List<ExerciseAlternative> Alternatives { get; set; } = [];

    public bool CanChoose(bool coach, bool planLocked) => coach || !(planLocked || ChoiceLocked);
}
