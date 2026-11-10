using Coaching.Application;
using Coaching.Application.Privacy;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class ClientAccountController
{
    [HttpGet("export")]
    public async Task<ActionResult<ExportResponse>> Export([FromServices] ExportDataHandler handler, [FromServices] IActor actor, CancellationToken ct) => Respond(await handler.Handle(actor.UserId!, ct));
}
