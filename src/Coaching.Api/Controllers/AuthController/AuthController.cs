using Coaching.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Coaching.Api.Controllers;

[Route("api/v1/auth")]
[EnableRateLimiting("auth")]
[ServiceFilter(typeof(AuthOriginFilter))]
public sealed partial class AuthController : ApiController
{
}
