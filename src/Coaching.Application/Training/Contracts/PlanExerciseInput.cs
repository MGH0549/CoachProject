
namespace Coaching.Application.Training;

public record PlanExerciseInput(Guid ExerciseId, int Sets, string Reps, int RestSeconds, string? Tempo, string? Note, bool ChoiceLocked, int? SelectedAlternative, List<AlternativeInput> Alternatives);
