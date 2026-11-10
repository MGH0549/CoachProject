using Coaching.Application.Progress;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class ClientProgressController
{
    [HttpPost("check-ins/{id:guid}/photos")]
    [Consumes("image/jpeg", "image/png", "image/webp")]
    [RequestSizeLimit(8 * 1024 * 1024)]
    public async Task<ActionResult<PhotoResponse>> UploadPhoto(Guid id, [FromServices] UploadPhotoHandler handler, CancellationToken ct) => Respond(await handler.Handle(id, Request.Body, ct));
}
