using Coaching.Application.Clients;
using Coaching.Application.Diet;
using Coaching.Application.Progress;
using Coaching.Application.Training;

namespace Coaching.Application.Privacy;

public record ExportResponse(AccountResponse Account, DateTimeOffset ConsentAt, TrainingResponse? Training, DietResponse? Diet, List<CheckInResponse> CheckIns, List<WorkoutResponse> Workouts);
