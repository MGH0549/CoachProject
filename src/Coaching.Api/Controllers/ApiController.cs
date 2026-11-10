using Coaching.Application;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

[ApiController]
[Produces("application/json")]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status429TooManyRequests)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)]
public abstract class ApiController : ControllerBase
{
    protected ActionResult<T> Respond<T>(Result<T> result)
    {
        if (result.Error is not { } error)
            return Ok(result.Value);
        return Problem(statusCode: error.Status, title: error.Message, extensions: new Dictionary<string, object?> { ["code"] = error.Code });
    }
}
