
namespace Coaching.Application;

public partial record Result<T>
{
    public static Result<T> Ok(T value) => new(value);
}
