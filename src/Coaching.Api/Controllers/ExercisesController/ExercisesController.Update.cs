using Coaching.Application.Exercises;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class ExercisesController
{
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ExerciseResponse>> Update(Guid id, ExerciseRequest request, [FromServices] SaveExerciseHandler handler, CancellationToken ct) => Respond(await handler.Handle(id, request, ct));
}
