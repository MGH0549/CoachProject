using Coaching.Application;
using Coaching.Application.Progress;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class ClientProgressController
{
    [HttpDelete("photos/{id:guid}")]
    public async Task<ActionResult<Done>> DeletePhoto(Guid id, [FromServices] DeletePhotoHandler handler, [FromServices] IActor actor, CancellationToken ct) => Respond(await handler.Handle(actor.UserId!, id, ct));
}
