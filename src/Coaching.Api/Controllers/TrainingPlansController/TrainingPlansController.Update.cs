using Coaching.Application.Training;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class TrainingPlansController
{
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TrainingResponse>> Update(Guid id, TrainingRequest request, [FromServices] SaveTrainingHandler handler, CancellationToken ct) => Respond(await handler.Handle(id, request, ct));
}
