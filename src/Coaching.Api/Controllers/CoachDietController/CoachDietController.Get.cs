using Coaching.Application.Diet;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class CoachDietController
{
    [HttpGet]
    public async Task<ActionResult<DietResponse>> Get(string clientId, [FromServices] ReadDietHandler handler, CancellationToken ct) => Respond(await handler.Handle(clientId, ct));
}
