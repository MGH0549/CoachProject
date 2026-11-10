
namespace Coaching.Application.Clients;

public record ExtendRequest(DateOnly EndDate, byte[] RowVersion);
