using Coaching.Api;
using Coaching.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.AddDevelopmentDefaults();
builder.ConfigureLoggingAndLimits();
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment.IsDevelopment());
builder.Services.AddRequestHandling();
builder.Services.AddTransportSecurity(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAccountAuthorization();
builder.Services.AddRequestRateLimiting();

var app = builder.Build();
if (args.Contains("--initialize", StringComparer.Ordinal))
{
    using var scope = app.Services.CreateScope();
    await Bootstrap.Initialize(scope.ServiceProvider, builder.Configuration);
    return;
}
app.UseCoachingPipeline();
app.MapControllers();
await app.RunAsync();

public partial class Program;
