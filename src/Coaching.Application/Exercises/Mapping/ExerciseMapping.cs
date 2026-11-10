using Coaching.Domain;

namespace Coaching.Application.Exercises;

public static class ExerciseMapping
{
    public static ExerciseResponse Response(this Exercise x) => new(x.Id, x.Name, x.MuscleGroup, x.YouTubeVideoId, x.Notes, x.Archived, x.RowVersion);
}
