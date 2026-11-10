using Coaching.Application.Training;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class CoachTrainingController
{
    [HttpPut("exercises/{id:guid}/selection")]
    public async Task<ActionResult<TrainingResponse>> Choose(string clientId, Guid id, ChooseExerciseRequest request, [FromServices] ChooseExerciseHandler handler, CancellationToken ct) => Respond(await handler.Handle(clientId, id, request, ct));
}
