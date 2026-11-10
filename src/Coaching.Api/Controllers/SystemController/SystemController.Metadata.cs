using Coaching.Infrastructure.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Coaching.Api.Controllers;

public sealed partial class SystemController
{
    [HttpGet("api/v1/metadata")]
    public ActionResult<MetadataResponse> Metadata([FromServices] IOptions<PlatformOptions> options) => Ok(new MetadataResponse(options.Value.TimeZone, options.Value.PrivacyPolicyUrl));
}
