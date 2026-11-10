
namespace Coaching.Application.Progress;

public record ProgressResponse(Page<CheckInResponse> CheckIns, Page<WorkoutResponse> Workouts, List<DayTotal> DailyWorkouts);
