
namespace Coaching.Application.Training;

public record TrainingRequest(string? ClientId, bool IsTemplate, string Name, bool ChoicesLocked, List<DayInput> Days, byte[]? RowVersion);
