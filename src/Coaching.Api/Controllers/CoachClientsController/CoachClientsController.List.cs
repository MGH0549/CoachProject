using Coaching.Application;
using Coaching.Application.Clients;
using Coaching.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

public sealed partial class CoachClientsController
{
    [HttpGet]
    public async Task<ActionResult<Page<AccountResponse>>> List(int? page, int? size, ClientStatus? status, string? search, [FromServices] ListClientsHandler handler, CancellationToken ct)
    {
        if (status is not null && !Enum.IsDefined(status.Value))
            return BadRequest(new ProblemDetails { Status = 400, Title = "حالة الحساب غير صالحة", Extensions = { ["code"] = "invalid_status" } });
        return Ok(await handler.Handle(page ?? 1, size ?? 20, status, search, ct));
    }
}
