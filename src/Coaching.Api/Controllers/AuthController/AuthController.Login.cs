using Coaching.Application.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class AuthController
{
    [HttpPost("login")]
    public async Task<ActionResult<SessionResponse>> Login(LoginRequest request, [FromServices] LoginHandler handler, CancellationToken ct) => SetSession(await handler.Handle(request, ct));
}
