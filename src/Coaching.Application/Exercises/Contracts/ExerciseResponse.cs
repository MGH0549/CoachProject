
namespace Coaching.Application.Exercises;

public record ExerciseResponse(Guid Id, string Name, string MuscleGroup, string YouTubeVideoId, string? Notes, bool Archived, byte[] RowVersion);
