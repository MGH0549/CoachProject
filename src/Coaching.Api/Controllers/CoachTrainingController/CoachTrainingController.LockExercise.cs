using Coaching.Application.Training;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class CoachTrainingController
{
    [HttpPut("exercises/{id:guid}/lock")]
    public async Task<ActionResult<TrainingResponse>> LockExercise(string clientId, Guid id, LockRequest request, [FromServices] LockExerciseHandler handler, CancellationToken ct) => Respond(await handler.Handle(clientId, id, request, ct));
}
