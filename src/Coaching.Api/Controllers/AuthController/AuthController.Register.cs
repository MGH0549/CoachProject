using Coaching.Application.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class AuthController
{
    [HttpPost("register")]
    public async Task<ActionResult<string>> Register(RegisterRequest request, [FromServices] RegisterHandler handler, CancellationToken ct) =>
     Respond(await handler.Handle(request, ct));
}
