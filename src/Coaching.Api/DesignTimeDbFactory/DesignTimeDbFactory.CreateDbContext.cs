using Coaching.Infrastructure;
using Coaching.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Coaching.Api;

public sealed partial class DesignTimeDbFactory
{
    public CoachingDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder().AddUserSecrets<Program>(optional: true).AddEnvironmentVariables().Build();
        var connection = configuration.GetConnectionString("SqlServer");
        if (string.IsNullOrWhiteSpace(connection)) connection = "Server=localhost;Database=Coaching;Integrated Security=true;Encrypt=true;TrustServerCertificate=true";
        return new(new DbContextOptionsBuilder<CoachingDbContext>().UseSqlServer(connection).Options,
            new DesignActor(), new CairoTime(TimeProvider.System, TimeZoneInfo.FindSystemTimeZoneById("Africa/Cairo")));
    }
}
