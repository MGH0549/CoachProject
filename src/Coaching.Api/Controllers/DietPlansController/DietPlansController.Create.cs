using Coaching.Application.Diet;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class DietPlansController
{
    [HttpPost]
    public async Task<ActionResult<DietResponse>> Create(DietRequest request, [FromServices] SaveDietHandler handler, CancellationToken ct) => Respond(await handler.Handle(null, request, ct));
}
