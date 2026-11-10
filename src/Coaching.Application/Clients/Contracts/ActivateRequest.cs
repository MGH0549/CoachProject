
namespace Coaching.Application.Clients;

public record ActivateRequest(string Package, DateOnly EndDate, byte[] RowVersion);
