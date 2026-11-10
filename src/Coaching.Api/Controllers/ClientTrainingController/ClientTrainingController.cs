using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coaching.Api.Controllers;

[Route("api/v1/me/training")]
[Authorize(Policy = "ActiveClient")]
public sealed partial class ClientTrainingController : ApiController
{
}
