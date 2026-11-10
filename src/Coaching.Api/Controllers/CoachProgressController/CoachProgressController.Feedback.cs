using Coaching.Application.Progress;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class CoachProgressController
{
    [HttpPut("check-ins/{id:guid}/feedback")]
    public async Task<ActionResult<CheckInResponse>> Feedback(string clientId, Guid id, FeedbackRequest request, [FromServices] FeedbackHandler handler, CancellationToken ct) => Respond(await handler.Handle(clientId, id, request, ct));
}
