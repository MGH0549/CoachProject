
namespace Coaching.Application;

public partial record Result<T>(T? Value, Error? Error = null)
{
    public bool IsSuccess => Error is null;
}
