using Coaching.Application;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Coaching.Api.Filters;

public sealed partial class RequestValidationFilter(IServiceProvider services, ICairoTime time) : IAsyncActionFilter
{
}
