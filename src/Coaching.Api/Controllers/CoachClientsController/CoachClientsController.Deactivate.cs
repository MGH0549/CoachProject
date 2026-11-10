using Coaching.Application.Clients;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class CoachClientsController
{
    [HttpPost("{clientId}/deactivate")]
    public async Task<ActionResult<AccountResponse>> Deactivate(string clientId, VersionRequest request, [FromServices] DeactivateClientHandler handler, CancellationToken ct) => Respond(await handler.Handle(clientId, request, ct));
}
