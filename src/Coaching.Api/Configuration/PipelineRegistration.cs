using Microsoft.EntityFrameworkCore;

namespace Coaching.Api;

public static class PipelineRegistration
{
    public static WebApplication UseCoachingPipeline(this WebApplication app)
    {
        app.UseExceptionHandler();
        app.Use(async (context, next) =>
        {
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            context.Response.Headers["X-Frame-Options"] = "DENY";
            context.Response.Headers["Cache-Control"] = "no-store";
            context.Response.Headers["Content-Security-Policy"] = "default-src 'none'; frame-ancestors 'none'";
            context.Response.Headers["Referrer-Policy"] = "no-referrer";
            await next();
        });
        app.UseStatusCodePages(async context =>
        {
            var status = context.HttpContext.Response.StatusCode;
            await ApiResults.Problem(status, status switch { 401 => "authentication_required", 403 => "access_denied", 404 => "not_found", 429 => "rate_limited", _ => "request_failed" },
                status switch { 401 => "يرجى تسجيل الدخول", 403 => "لا تملك صلاحية الوصول أو انتهى اشتراكك", 404 => "البيانات غير موجودة", 429 => "طلبات كثيرة. حاول لاحقاً", _ => "تعذر تنفيذ الطلب" }).ExecuteAsync(context.HttpContext);
        });
        if (!app.Environment.IsDevelopment()) app.UseHsts();
        app.UseHttpsRedirection(); app.UseResponseCompression(); app.UseCors(); app.UseAuthentication(); app.UseRateLimiter(); app.UseAuthorization();
        if (app.Environment.IsDevelopment()) app.MapOpenApi();
        else app.MapOpenApi().RequireAuthorization("Coach");
        return app;
    }
}
