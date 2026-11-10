
namespace Coaching.Application.Training;

public record DayResponse(Guid Id, string Name, int Order, List<PlanExerciseResponse> Exercises);
