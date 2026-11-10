using Coaching.Application.Diet;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class TemplatesController
{
    [HttpGet("diet/{id:guid}")]
    public async Task<ActionResult<DietResponse>> Diet(Guid id, [FromServices] ReadDietHandler handler, CancellationToken ct) => Respond(await handler.Template(id, ct));
}
