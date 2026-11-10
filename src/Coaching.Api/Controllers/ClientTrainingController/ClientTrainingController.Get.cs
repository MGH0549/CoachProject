using Coaching.Application;
using Coaching.Application.Training;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class ClientTrainingController
{
    [HttpGet]
    public async Task<ActionResult<TrainingResponse>> Get([FromServices] ReadTrainingHandler handler, [FromServices] IActor actor, CancellationToken ct) => Respond(await handler.Handle(actor.UserId!, ct));
}
