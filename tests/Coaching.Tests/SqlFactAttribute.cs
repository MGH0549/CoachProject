using Xunit;

namespace Coaching.Tests;

public sealed class SqlFactAttribute : FactAttribute
{
    public SqlFactAttribute()
    {
        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("TestSqlServer")))
            Skip = "Set TestSqlServer to a SQL Server connection string with permission to create isolated CoachingTests_* databases.";
    }
}
