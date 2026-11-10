using Coaching.Application;
using Coaching.Application.Clients;
using Coaching.Application.Privacy;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class CoachClientsController
{
    [HttpDelete("{clientId}")]
    public async Task<ActionResult<Done>> Delete(string clientId, [FromBody] VersionRequest request, [FromServices] DeleteDataHandler handler, CancellationToken ct) => Respond(await handler.Handle(clientId, request.RowVersion, ct));
}
