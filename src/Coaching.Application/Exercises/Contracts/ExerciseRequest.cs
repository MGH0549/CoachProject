
namespace Coaching.Application.Exercises;

public record ExerciseRequest(string Name, string MuscleGroup, string YouTubeVideoId, string? Notes, bool Archived, byte[]? RowVersion);
