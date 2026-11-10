using Coaching.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Design;

namespace Coaching.Api;

public sealed partial class DesignTimeDbFactory : IDesignTimeDbContextFactory<CoachingDbContext>
{
}
