using Coaching.Application.Diet;
using Coaching.Application.Training;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class CoachDietController
{
    [HttpPut("meals/{id:guid}/lock")]
    public async Task<ActionResult<DietResponse>> LockMeal(string clientId, Guid id, LockRequest request, [FromServices] LockMealHandler handler, CancellationToken ct) => Respond(await handler.Handle(clientId, id, request, ct));
}
