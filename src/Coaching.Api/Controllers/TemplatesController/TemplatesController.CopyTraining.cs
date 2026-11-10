using Coaching.Application.Training;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class TemplatesController
{
    [HttpPost("training/{id:guid}/copy")]
    public async Task<ActionResult<TrainingResponse>> CopyTraining(Guid id, CopyTemplateRequest request, [FromServices] CopyTrainingHandler handler, CancellationToken ct) => Respond(await handler.Handle(id, request, ct));
}
