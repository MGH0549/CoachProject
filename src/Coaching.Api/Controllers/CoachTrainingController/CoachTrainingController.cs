using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

[Route("api/v1/coach/clients/{clientId}/training")]
[Authorize(Policy = "Coach")]
public sealed partial class CoachTrainingController : ApiController
{
}
