using Coaching.Application.Progress;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class ClientProgressController
{
    [HttpPost("check-ins")]
    public async Task<ActionResult<CheckInResponse>> SubmitCheckIn(CheckInRequest request, [FromServices] SubmitCheckInHandler handler, CancellationToken ct) => Respond(await handler.Handle(request, ct));
}
