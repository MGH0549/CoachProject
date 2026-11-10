using Coaching.Application;
using Coaching.Application.Exercises;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Coaching.Api.Controllers;

public sealed partial class ExercisesController
{
    [HttpGet]
    public async Task<ActionResult<Page<ExerciseResponse>>> List(int? page, int? size, string? search, bool? archived, [FromServices] ListExercisesHandler handler, [FromServices] IExerciseCache version, [FromServices] IMemoryCache cache, CancellationToken ct)
    {
        var key = ("exercises", version.Version, page ?? 1, size ?? 20, search, archived ?? false);
        var result = await cache.GetOrCreateAsync(key, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            return await handler.Handle(page ?? 1, size ?? 20, search, archived ?? false, ct);
        });
        return Ok(result);
    }
}
