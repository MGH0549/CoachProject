using Coaching.Infrastructure.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Coaching.Api.Filters;

public sealed class AuthOriginFilter(IOptions<PlatformOptions> options) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var request = context.HttpContext.Request;
        if (request.Headers["X-Coaching-Client"] != "Angular" || request.Headers.TryGetValue("Origin", out var supplied) && supplied != options.Value.AngularOrigin)
        {
            context.Result = new ObjectResult(new ProblemDetails { Status = 403, Title = "مصدر الطلب غير مسموح", Extensions = { ["code"] = "invalid_origin" } })
            {
                StatusCode = 403
            };
            return;
        }

        await next();
    }
}
