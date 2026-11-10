using Coaching.Application.Exercises;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class ExercisesController
{
    [HttpPost]
    public async Task<ActionResult<ExerciseResponse>> Create(ExerciseRequest request, [FromServices] SaveExerciseHandler handler, CancellationToken ct) => Respond(await handler.Handle(null, request, ct));
}
