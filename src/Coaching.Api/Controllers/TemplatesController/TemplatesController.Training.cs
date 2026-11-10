using Coaching.Application.Training;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class TemplatesController
{
    [HttpGet("training/{id:guid}")]
    public async Task<ActionResult<TrainingResponse>> Training(Guid id, [FromServices] ReadTrainingHandler handler, CancellationToken ct) => Respond(await handler.Template(id, ct));
}
