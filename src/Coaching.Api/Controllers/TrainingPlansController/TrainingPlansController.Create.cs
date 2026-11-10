using Coaching.Application.Training;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class TrainingPlansController
{
    [HttpPost]
    public async Task<ActionResult<TrainingResponse>> Create(TrainingRequest request, [FromServices] SaveTrainingHandler handler, CancellationToken ct) => Respond(await handler.Handle(null, request, ct));
}
