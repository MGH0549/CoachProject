using Coaching.Application.Training;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class CoachTrainingController
{
    [HttpGet]
    public async Task<ActionResult<TrainingResponse>> Get(string clientId, [FromServices] ReadTrainingHandler handler, CancellationToken ct) => Respond(await handler.Handle(clientId, ct));
}
