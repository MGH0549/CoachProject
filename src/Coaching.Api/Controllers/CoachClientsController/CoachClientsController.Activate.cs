using Coaching.Application.Clients;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class CoachClientsController
{
    [HttpPost("{clientId}/activate")]
    public async Task<ActionResult<AccountResponse>> Activate(string clientId, ActivateRequest request, [FromServices] ActivateClientHandler handler, CancellationToken ct) => Respond(await handler.Handle(clientId, request, ct));
}
