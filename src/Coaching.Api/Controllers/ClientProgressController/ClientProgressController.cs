using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

[Route("api/v1/me")]
[Authorize(Policy = "ActiveClient")]
public sealed partial class ClientProgressController : ApiController
{
}
