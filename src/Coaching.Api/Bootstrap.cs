using Coaching.Domain;
using Coaching.Infrastructure.Persistence;
using Coaching.Infrastructure.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Coaching.Api;

public static class Bootstrap
{
    public static async Task Initialize(IServiceProvider services, IConfiguration config)
    {
        var email = config["Bootstrap:Email"] ?? throw new InvalidOperationException("Bootstrap:Email is required.");
        var password = config["Bootstrap:Password"] ?? throw new InvalidOperationException("Bootstrap:Password is required.");
        var db = services.GetRequiredService<CoachingDbContext>();
        await db.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
        {
            db.ChangeTracker.Clear();
            await using var transaction = await db.Database.BeginTransactionAsync();
            await db.Database.ExecuteSqlRawAsync("DECLARE @r int; EXEC @r = sp_getapplock @Resource='coaching:bootstrap', @LockMode='Exclusive', @LockOwner='Transaction', @LockTimeout=30000; IF @r < 0 THROW 51000, 'Coach initialization is busy.', 1;");
            var roles = services.GetRequiredService<RoleManager<IdentityRole>>();
            foreach (var name in new[] { Roles.Coach, Roles.Client })
                if (!await roles.RoleExistsAsync(name) && !(await roles.CreateAsync(new(name))).Succeeded)
                    throw new InvalidOperationException("Could not initialize roles.");
            var users = services.GetRequiredService<UserManager<AppUser>>();
            var coaches = await users.GetUsersInRoleAsync(Roles.Coach);
            if (coaches.Count > 0)
            {
                if (coaches.Count == 1 && string.Equals(coaches[0].Email, email, StringComparison.OrdinalIgnoreCase))
                    return;
                throw new InvalidOperationException("A coach account already exists.");
            }

            if (await users.FindByEmailAsync(email) is not null)
                throw new InvalidOperationException("That email already belongs to another account.");
            var user = new AppUser
            {
                UserName = email,
                Email = email
            };
            if (!(await users.CreateAsync(user, password)).Succeeded)
                throw new InvalidOperationException("Coach creation failed. Check the password policy.");
            if (!(await users.AddToRoleAsync(user, Roles.Coach)).Succeeded)
                throw new InvalidOperationException("Coach role assignment failed.");
            await transaction.CommitAsync();
        });
    }
}
