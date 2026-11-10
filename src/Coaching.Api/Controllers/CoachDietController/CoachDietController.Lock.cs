using Coaching.Application.Diet;
using Coaching.Application.Training;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class CoachDietController
{
    [HttpPut("lock")]
    public async Task<ActionResult<DietResponse>> Lock(string clientId, LockRequest request, [FromServices] LockMealHandler handler, CancellationToken ct) => Respond(await handler.Handle(clientId, null, request, ct));
}
