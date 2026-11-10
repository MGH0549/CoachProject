using Coaching.Application;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class AuthController
{
    [HttpPost("logout")]
    public async Task<ActionResult<Done>> Logout([FromServices] IAccounts accounts, CancellationToken ct)
    {
        await accounts.Logout(Request.Cookies[RefreshCookie.Name] ?? "", ct);
        Response.Cookies.Delete(RefreshCookie.Name, RefreshCookie.Options());
        return Ok(new Done());
    }
}
