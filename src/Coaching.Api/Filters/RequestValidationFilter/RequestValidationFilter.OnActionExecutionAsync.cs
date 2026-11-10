using System.Globalization;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Coaching.Api.Filters;

public sealed partial class RequestValidationFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var query = context.HttpContext.Request.Query;
        if (query.TryGetValue("page", out var page) && (!int.TryParse(page, out var pn) || pn is < 1 or > 100000) || query.TryGetValue("size", out var size) && (!int.TryParse(size, out var ps) || ps is < 1 or > 100) || query.TryGetValue("search", out var search) && search.ToString().Length > 200)
        {
            context.Result = Failure("invalid_pagination", "قيم البحث أو ترقيم الصفحات غير صالحة");
            return;
        }

        var from = query.TryGetValue("from", out var f) && DateOnly.TryParse(f, CultureInfo.InvariantCulture, DateTimeStyles.None, out var fd) ? fd : time.Today.AddDays(-90);
        var to = query.TryGetValue("to", out var t) && DateOnly.TryParse(t, CultureInfo.InvariantCulture, DateTimeStyles.None, out var td) ? td : time.Today;
        if (from > to || to.DayNumber - from.DayNumber > 366)
        {
            context.Result = Failure("invalid_date_range", "اختر فترة لا تزيد عن سنة");
            return;
        }

        foreach (var value in context.ActionArguments.Values)
        {
            if (value is null || services.GetService(typeof(IValidator<>).MakeGenericType(value.GetType())) is not IValidator validator)
                continue;
            var result = await validator.ValidateAsync(new ValidationContext<object>(value), context.HttpContext.RequestAborted);
            if (result.IsValid)
                continue;
            var problem = new ProblemDetails
            {
                Status = 400,
                Title = "يرجى مراجعة البيانات المدخلة"
            };
            problem.Extensions["code"] = "validation_failed";
            problem.Extensions["errors"] = result.Errors.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).Distinct().ToArray());
            context.Result = new BadRequestObjectResult(problem);
            return;
        }

        await next();
    }
}
