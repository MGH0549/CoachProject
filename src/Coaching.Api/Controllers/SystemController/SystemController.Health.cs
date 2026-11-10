using Coaching.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class SystemController
{
    [HttpGet("health")]
    public async Task<ActionResult<HealthResponse>> Health([FromServices] CoachingDbContext db, CancellationToken ct) => await db.Database.CanConnectAsync(ct) ? Ok(new HealthResponse("healthy")) : Problem(statusCode: 503, title: "الخدمة غير متاحة", extensions: new Dictionary<string, object?> { ["code"] = "database_unavailable" });
}
