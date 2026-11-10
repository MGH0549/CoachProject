using Coaching.Application.Privacy;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class CoachClientsController
{
    [HttpGet("{clientId}/export")]
    public async Task<ActionResult<ExportResponse>> Export(string clientId, [FromServices] ExportDataHandler handler, CancellationToken ct) => Respond(await handler.Handle(clientId, ct));
}
