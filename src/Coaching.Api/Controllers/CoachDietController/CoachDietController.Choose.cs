using Coaching.Application.Diet;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class CoachDietController
{
    [HttpPut("meals/{id:guid}/selection")]
    public async Task<ActionResult<DietResponse>> Choose(string clientId, Guid id, ChooseMealRequest request, [FromServices] ChooseMealHandler handler, CancellationToken ct) => Respond(await handler.Handle(clientId, id, request, ct));
}
