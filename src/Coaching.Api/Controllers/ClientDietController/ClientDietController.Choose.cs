using Coaching.Application;
using Coaching.Application.Diet;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class ClientDietController
{
    [HttpPut("meals/{id:guid}/selection")]
    public async Task<ActionResult<DietResponse>> Choose(Guid id, ChooseMealRequest request, [FromServices] ChooseMealHandler handler, [FromServices] IActor actor, CancellationToken ct) => Respond(await handler.Handle(actor.UserId!, id, request, ct));
}
