using Coaching.Application;
using Coaching.Application.Diet;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class ClientDietController
{
    [HttpGet]
    public async Task<ActionResult<DietResponse>> Get([FromServices] ReadDietHandler handler, [FromServices] IActor actor, CancellationToken ct) => Respond(await handler.Handle(actor.UserId!, ct));
}
