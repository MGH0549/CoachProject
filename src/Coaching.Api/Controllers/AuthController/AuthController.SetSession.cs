using Coaching.Application;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class AuthController
{
    private ActionResult<SessionResponse> SetSession(Result<Session> result)
    {
        if (!result.IsSuccess)
        {
            Response.Cookies.Delete(RefreshCookie.Name, RefreshCookie.Options());
            return Respond(new Result<SessionResponse>(null, result.Error));
        }

        var session = result.Value!;
        var options = RefreshCookie.Options();
        options.Expires = session.RefreshExpiresAt;
        Response.Cookies.Append(RefreshCookie.Name, session.RefreshToken, options);
        return Ok(new SessionResponse(session.AccessToken, session.ExpiresAt));
    }
}
