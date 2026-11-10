using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

[Route("api/v1/me/diet")]
[Authorize(Policy = "ActiveClient")]
public sealed partial class ClientDietController : ApiController
{
}
