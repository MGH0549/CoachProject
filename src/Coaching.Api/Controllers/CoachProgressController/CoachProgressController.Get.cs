using Coaching.Application;
using Coaching.Application.Progress;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class CoachProgressController
{
    [HttpGet("progress")]
    public async Task<ActionResult<ProgressResponse>> Get(string clientId, int? page, int? size, DateOnly? from, DateOnly? to, [FromServices] ReadProgressHandler handler, [FromServices] ICairoTime time, CancellationToken ct) => Ok(await handler.Handle(clientId, page ?? 1, size ?? 20, from ?? time.Today.AddDays(-90), to ?? time.Today, ct));
}
