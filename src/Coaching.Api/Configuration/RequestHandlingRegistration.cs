using System.Globalization;
using System.Text.Json.Serialization;
using Coaching.Api.Filters;
using Coaching.Application.Auth;
using FluentValidation;

namespace Coaching.Api;

public static class RequestHandlingRegistration
{
    public static IServiceCollection AddRequestHandling(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<RegisterValidator>();
        ValidatorOptions.Global.LanguageManager.Culture = CultureInfo.GetCultureInfo("ar");
        foreach (var handler in typeof(RegisterHandler).Assembly.GetTypes().Where(x => x.IsClass && !x.IsAbstract && x.Name.EndsWith("Handler", StringComparison.Ordinal)))
            services.AddScoped(handler);
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        services.AddScoped<AuthOriginFilter>();
        services.AddScoped<RequestValidationFilter>();
        services.AddControllers(o => { o.Filters.AddService<RequestValidationFilter>(); o.Filters.Add<ProblemResponseFilter>(); }).AddJsonOptions(o =>
        { o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); o.JsonSerializerOptions.Converters.Add(new UtcDateTimeOffsetConverter()); });
        services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(o => o.InvalidModelStateResponseFactory = _ =>
            new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(new Microsoft.AspNetCore.Mvc.ProblemDetails
            { Status = 400, Title = "صيغة الطلب أو البيانات غير صالحة", Extensions = { ["code"] = "validation_failed" } }));
        services.AddOpenApi();
        services.AddProblemDetails();
        services.AddExceptionHandler<ApiExceptionHandler>();
        services.AddResponseCompression(o => o.EnableForHttps = true);
        services.AddMemoryCache();
        return services;
    }
}
