using Coaching.Application;
using Coaching.Application.Progress;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class ClientProgressController
{
    [HttpGet("progress")]
    public async Task<ActionResult<ProgressResponse>> Get(int? page, int? size, DateOnly? from, DateOnly? to, [FromServices] ReadProgressHandler handler, [FromServices] IActor actor, [FromServices] ICairoTime time, CancellationToken ct) => Ok(await handler.Handle(actor.UserId!, page ?? 1, size ?? 20, from ?? time.Today.AddDays(-90), to ?? time.Today, ct));
}
