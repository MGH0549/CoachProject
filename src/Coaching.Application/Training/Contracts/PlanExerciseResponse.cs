using Coaching.Application.Exercises;

namespace Coaching.Application.Training;

public record PlanExerciseResponse(Guid Id, ExerciseResponse Exercise, int Sets, string Reps, int RestSeconds, string? Tempo, string? Note, int Order, bool ChoiceLocked, Guid? SelectedAlternativeId, List<AlternativeResponse> Alternatives, byte[] RowVersion);
