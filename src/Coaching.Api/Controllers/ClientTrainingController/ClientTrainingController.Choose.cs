using Coaching.Application;
using Coaching.Application.Training;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class ClientTrainingController
{
    [HttpPut("exercises/{id:guid}/selection")]
    public async Task<ActionResult<TrainingResponse>> Choose(Guid id, ChooseExerciseRequest request, [FromServices] ChooseExerciseHandler handler, [FromServices] IActor actor, CancellationToken ct) => Respond(await handler.Handle(actor.UserId!, id, request, ct));
}
