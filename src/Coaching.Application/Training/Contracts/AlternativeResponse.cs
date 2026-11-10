using Coaching.Application.Exercises;

namespace Coaching.Application.Training;

public record AlternativeResponse(Guid Id, ExerciseResponse Exercise, int Sets, string Reps, string? Note);
