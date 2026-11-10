using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Coaching.Api.Filters;

public sealed class ProblemResponseFilter : IAsyncAlwaysRunResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult { Value: ProblemDetails problem } && !problem.Extensions.ContainsKey("code"))
        {
            problem.Extensions["code"] = problem.Status switch
            {
                400 => "invalid_request", 401 => "authentication_required", 403 => "access_denied",
                404 => "not_found", 405 => "method_not_allowed", 413 => "request_too_large",
                415 => "unsupported_media_type", 429 => "rate_limited", _ => "request_failed"
            };
            problem.Title = "تعذر تنفيذ الطلب. يرجى مراجعة البيانات والصلاحيات";
        }
        await next();
    }
}
