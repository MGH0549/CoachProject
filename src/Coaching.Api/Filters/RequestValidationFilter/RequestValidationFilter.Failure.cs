using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Filters;

public sealed partial class RequestValidationFilter
{
    private static BadRequestObjectResult Failure(string code, string message) => new(new ProblemDetails { Status = 400, Title = message, Extensions = { ["code"] = code } });
}
