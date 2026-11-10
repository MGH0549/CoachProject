
namespace Coaching.Application.Training;

public record TrainingResponse(Guid Id, string? ClientId, bool IsTemplate, string Name, bool ChoicesLocked, List<DayResponse> Days, byte[] RowVersion);
