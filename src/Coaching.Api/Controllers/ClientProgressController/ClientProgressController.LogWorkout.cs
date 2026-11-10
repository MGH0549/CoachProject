using Coaching.Application.Progress;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class ClientProgressController
{
    [HttpPost("workouts")]
    public async Task<ActionResult<WorkoutResponse>> LogWorkout(WorkoutRequest request, [FromServices] LogWorkoutHandler handler, CancellationToken ct) => Respond(await handler.Handle(request, ct));
}
