using Coaching.Application;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class AuthController
{
    [HttpPost("refresh")]
    public async Task<ActionResult<SessionResponse>> Refresh([FromServices] IAccounts accounts, CancellationToken ct) => SetSession(await accounts.Refresh(Request.Cookies[RefreshCookie.Name] ?? "", ct));
}
