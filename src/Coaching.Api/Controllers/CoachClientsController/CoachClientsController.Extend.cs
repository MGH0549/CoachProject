using Coaching.Application.Clients;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class CoachClientsController
{
    [HttpPost("{clientId}/extend")]
    public async Task<ActionResult<AccountResponse>> Extend(string clientId, ExtendRequest request, [FromServices] ExtendClientHandler handler, CancellationToken ct) => Respond(await handler.Handle(clientId, request, ct));
}
