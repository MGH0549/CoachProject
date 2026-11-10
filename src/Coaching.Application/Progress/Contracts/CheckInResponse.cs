
namespace Coaching.Application.Progress;

public record CheckInResponse(Guid Id, DateTimeOffset SubmittedAt, DateOnly CairoDate, decimal Weight, decimal? Waist, decimal? Chest, decimal? Hips, int Energy, string? Notes, string? CoachFeedback, List<PhotoResponse> Photos, byte[] RowVersion);
