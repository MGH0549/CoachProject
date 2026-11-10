using Coaching.Application;
using Coaching.Application.Clients;
using Coaching.Infrastructure.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Coaching.Api.Controllers;

public sealed partial class ClientAccountController
{
    [HttpGet]
    public async Task<ActionResult<MyAccountResponse>> Get([FromServices] MyAccountHandler handler, [FromServices] IOptions<PlatformOptions> options, CancellationToken ct)
    {
        var result = await handler.Handle(ct);
        if (!result.IsSuccess)
            return Respond(new Result<MyAccountResponse>(null, result.Error));
        var message = Uri.EscapeDataString("مرحباً، أريد تفعيل حسابي في منصة التدريب. بريدي الإلكتروني: " + result.Value!.Email);
        var link = string.IsNullOrWhiteSpace(options.Value.CoachWhatsApp) ? "" : $"https://wa.me/{options.Value.CoachWhatsApp}?text={message}";
        return Ok(new MyAccountResponse(result.Value, link, options.Value.TimeZone, options.Value.CheckInDueDay));
    }
}
