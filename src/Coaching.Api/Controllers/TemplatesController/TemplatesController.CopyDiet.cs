using Coaching.Application.Diet;
using Coaching.Application.Training;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class TemplatesController
{
    [HttpPost("diet/{id:guid}/copy")]
    public async Task<ActionResult<DietResponse>> CopyDiet(Guid id, CopyTemplateRequest request, [FromServices] CopyDietHandler handler, CancellationToken ct) => Respond(await handler.Handle(id, request, ct));
}
