using Coaching.Application.Diet;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class DietPlansController
{
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<DietResponse>> Update(Guid id, DietRequest request, [FromServices] SaveDietHandler handler, CancellationToken ct) => Respond(await handler.Handle(id, request, ct));
}
