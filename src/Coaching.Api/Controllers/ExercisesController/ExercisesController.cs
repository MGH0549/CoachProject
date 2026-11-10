using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

[Route("api/v1/coach/exercises")]
[Authorize(Policy = "Coach")]
public sealed partial class ExercisesController : ApiController
{
}
