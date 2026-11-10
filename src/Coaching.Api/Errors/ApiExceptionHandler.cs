using System.Text.Json;
using Coaching.Infrastructure.Photos;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Coaching.Api;

public sealed class ApiExceptionHandler(ILogger<ApiExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        var (status, code, message) = exception switch
        {
            DbUpdateConcurrencyException => (409, "concurrency_conflict", "تم تعديل البيانات. أعد تحميلها وحاول مجدداً"),
            DbUpdateException { InnerException: SqlException { Number: 2601 or 2627 } } => (409, "record_exists", "السجل موجود بالفعل"),
            DbUpdateException { InnerException: SqlException { Number: 547 } } => (409, "record_in_use", "السجل مرتبط ببيانات أخرى"),
            BadHttpRequestException e => (e.StatusCode, "invalid_request", "الطلب غير صالح"),
            JsonException => (400, "invalid_json", "صيغة البيانات غير صالحة"),
            PhotoServiceException or HttpRequestException => (503, "service_unavailable", "الخدمة غير متاحة مؤقتاً. حاول مجدداً"),
            _ => (500, "internal_error", "حدث خطأ غير متوقع")
        };
        if (status >= 500)
            logger.LogError("Request failed with {ErrorType}; trace {TraceId}", exception.GetType().Name, context.TraceIdentifier);
        await ApiResults.Problem(status, code, message).ExecuteAsync(context);
        return true;
    }
}
