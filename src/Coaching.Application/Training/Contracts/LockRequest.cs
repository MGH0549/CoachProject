
namespace Coaching.Application.Training;

public record LockRequest(bool Locked, byte[] RowVersion);
