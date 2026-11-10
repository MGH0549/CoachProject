using Coaching.Application.Training;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class CoachTrainingController
{
    [HttpPut("lock")]
    public async Task<ActionResult<TrainingResponse>> Lock(string clientId, LockRequest request, [FromServices] LockExerciseHandler handler, CancellationToken ct) => Respond(await handler.Handle(clientId, null, request, ct));
}
