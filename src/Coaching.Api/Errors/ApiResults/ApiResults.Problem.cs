
namespace Coaching.Api;

public static partial class ApiResults
{
    public static IResult Problem(int status, string code, string message) => Results.Problem(statusCode: status, title: message, extensions: new Dictionary<string, object?> { ["code"] = code });
}
