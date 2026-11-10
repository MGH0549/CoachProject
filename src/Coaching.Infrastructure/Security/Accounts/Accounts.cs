using Coaching.Application;
using Coaching.Infrastructure.Persistence;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Coaching.Infrastructure.Security;

public sealed partial class Accounts(CoachingDbContext db, UserManager<AppUser> users, IOptions<JwtOptions> jwt, IOptions<PlatformOptions> platform, ICairoTime time, IDataProtectionProvider protection) : IAccounts
{
}
