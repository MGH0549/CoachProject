using Coaching.Application;
using Coaching.Infrastructure.Persistence;
using Coaching.Infrastructure.Security;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Coaching.Infrastructure.Jobs;

public sealed partial class MaintenanceJobs(CoachingDbContext db, ICairoTime time, TimeZoneInfo zone, IOptions<PlatformOptions> platform, IPhotoStorage photos, IHttpClientFactory clients, IOptions<ResendOptions> resend, IDataProtectionProvider protection, ILogger<MaintenanceJobs> logger)
{
}
