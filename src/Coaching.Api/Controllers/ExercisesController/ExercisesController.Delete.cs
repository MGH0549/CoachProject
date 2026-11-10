using Coaching.Application;
using Coaching.Application.Clients;
using Coaching.Application.Exercises;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class ExercisesController
{
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Done>> Delete(Guid id, [FromBody] VersionRequest request, [FromServices] DeleteExerciseHandler handler, CancellationToken ct) => Respond(await handler.Handle(id, request.RowVersion, ct));
}
