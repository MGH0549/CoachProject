using Coaching.Application;
using Coaching.Application.Diet;
using Coaching.Application.Training;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class TemplatesController
{
    [HttpGet("diet")]
    public async Task<ActionResult<Page<TemplateResponse>>> DietList(int? page, int? size, [FromServices] ReadDietHandler handler, CancellationToken ct) => Ok(await handler.Templates(page ?? 1, size ?? 20, ct));
}
