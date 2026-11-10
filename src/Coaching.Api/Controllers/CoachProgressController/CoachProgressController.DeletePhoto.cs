using Coaching.Application;
using Coaching.Application.Progress;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class CoachProgressController
{
    [HttpDelete("photos/{id:guid}")]
    public async Task<ActionResult<Done>> DeletePhoto(string clientId, Guid id, [FromServices] DeletePhotoHandler handler, CancellationToken ct) => Respond(await handler.Handle(clientId, id, ct));
}
