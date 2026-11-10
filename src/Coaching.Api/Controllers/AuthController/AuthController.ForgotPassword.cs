using Coaching.Application;
using Coaching.Application.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class AuthController
{
    [HttpPost("forgot-password")]
    public async Task<ActionResult<Done>> ForgotPassword(ForgotPasswordRequest request, [FromServices] ForgotPasswordHandler handler, CancellationToken ct)
    {
        await handler.Handle(request, ct);
        return Ok(new Done());
    }
}
