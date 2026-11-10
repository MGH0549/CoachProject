
namespace Coaching.Application;

public record Error(string Code, string Message, int Status = 400);
