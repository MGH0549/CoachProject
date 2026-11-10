using Coaching.Application;
using Coaching.Application.Training;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class TemplatesController
{
    [HttpGet("training")]
    public async Task<ActionResult<Page<TemplateResponse>>> TrainingList(int? page, int? size, [FromServices] ReadTrainingHandler handler, CancellationToken ct) => Ok(await handler.Templates(page ?? 1, size ?? 20, ct));
}
