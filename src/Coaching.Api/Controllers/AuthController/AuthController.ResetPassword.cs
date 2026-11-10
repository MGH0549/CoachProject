using Coaching.Application;
using Coaching.Application.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class AuthController
{
    [HttpPost("reset-password")]
    public async Task<ActionResult<Done>> ResetPassword(ResetPasswordRequest request, [FromServices] ResetPasswordHandler handler, CancellationToken ct) => Respond(await handler.Handle(request, ct));
}
