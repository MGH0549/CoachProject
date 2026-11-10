using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

[Route("api/v1/coach/clients/{clientId}/diet")]
[Authorize(Policy = "Coach")]
public sealed partial class CoachDietController : ApiController
{
}
