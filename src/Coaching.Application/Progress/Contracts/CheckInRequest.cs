
namespace Coaching.Application.Progress;

public record CheckInRequest(decimal Weight, decimal? Waist, decimal? Chest, decimal? Hips, int Energy, string? Notes);
