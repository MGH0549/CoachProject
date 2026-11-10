using Coaching.Application;
using Coaching.Application.Clients;
using Coaching.Application.Privacy;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class ClientAccountController
{
    [HttpDelete]
    public async Task<ActionResult<Done>> Delete([FromBody] VersionRequest request, [FromServices] DeleteDataHandler handler, [FromServices] IActor actor, CancellationToken ct)
    {
        var result = await handler.Handle(actor.UserId!, request.RowVersion, ct);
        if (result.IsSuccess)
            Response.Cookies.Delete(RefreshCookie.Name, RefreshCookie.Options());
        return Respond(result);
    }
}
