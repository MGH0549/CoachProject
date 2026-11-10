
namespace Coaching.Application;

public partial record Result<T>
{
    public static Result<T> Fail(string code, string message, int status = 400) => new(default, new(code, message, status));
}
