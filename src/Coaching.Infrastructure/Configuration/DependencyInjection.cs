using Microsoft.EntityFrameworkCore;
using Coaching.Application;
using Coaching.Infrastructure.Jobs;
using Coaching.Infrastructure.Persistence;
using Coaching.Infrastructure.Photos;
using Coaching.Infrastructure.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coaching.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, bool isDevelopment = false)
    {
        services.AddOptions<JwtOptions>().Bind(configuration.GetSection("Jwt")).Validate(x => x.Key.Length >= 32 && x.AccessMinutes is >= 10 and <= 15 && x.RefreshDays is >= 1 and <= 90, "Jwt:Key (32+ characters) and valid token lifetimes are required.").ValidateOnStart();
        services.AddOptions<PlatformOptions>().Bind(configuration.GetSection("Platform"))
            .Validate(x => Uri.TryCreate(x.AngularOrigin, UriKind.Absolute, out var origin) && origin.Scheme == "https" && origin.AbsolutePath == "/", "Platform:AngularOrigin must be an HTTPS origin.")
            .Validate(x => x.TimeZone == "Africa/Cairo" && x.ReminderLeadDays is >= 1 and <= 30 && x.PhotoRetentionDays == 7 && x.AuditRetentionDays >= 1, "Invalid platform settings.")
            .Validate(x => isDevelopment && string.IsNullOrWhiteSpace(x.CoachWhatsApp) || System.Text.RegularExpressions.Regex.IsMatch(x.CoachWhatsApp, "^[0-9]{8,15}$"), "Platform:CoachWhatsApp must contain the international number, digits only.")
            .Validate(x => isDevelopment && string.IsNullOrWhiteSpace(x.PrivacyPolicyUrl) || Uri.TryCreate(x.PrivacyPolicyUrl, UriKind.Absolute, out var policy) && policy.Scheme == "https", "Platform:PrivacyPolicyUrl must be HTTPS.").ValidateOnStart();
        services.AddOptions<CloudinaryOptions>().Bind(configuration.GetSection("Cloudinary"))
            .Validate(x => x.LinkMinutes is >= 1 and <= 10 && (x.IsConfigured || isDevelopment && string.IsNullOrWhiteSpace(x.CloudName) && string.IsNullOrWhiteSpace(x.ApiKey) && string.IsNullOrWhiteSpace(x.ApiSecret)), "Cloudinary credentials and a short link lifetime are required.").ValidateOnStart();
        services.AddOptions<ResendOptions>().Bind(configuration.GetSection("Resend"))
            .Validate(x => x.IsConfigured || isDevelopment && string.IsNullOrWhiteSpace(x.ApiKey) && string.IsNullOrWhiteSpace(x.From), "Resend credentials and sender are required.").ValidateOnStart();
        services.AddSingleton(TimeProvider.System);
        services.AddSingleton(TimeZoneInfo.FindSystemTimeZoneById(configuration["Platform:TimeZone"] ?? "Africa/Cairo"));
        services.AddSingleton<ICairoTime, CairoTime>();
        services.AddHttpContextAccessor();
        services.AddScoped<RequestActor>();
        services.AddScoped<IActor>(s => s.GetRequiredService<RequestActor>());
        services.AddDbContext<CoachingDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServer"), sql => sql.EnableRetryOnFailure()));
        services.AddScoped<IStore>(s => s.GetRequiredService<CoachingDbContext>());
        services.AddIdentityCore<AppUser>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 12;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Lockout.MaxFailedAccessAttempts = 5;
        }).AddRoles<IdentityRole>().AddEntityFrameworkStores<CoachingDbContext>().AddDefaultTokenProviders();
        services.Configure<DataProtectionTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromHours(1));
        services.AddDataProtection();
        services.AddScoped<IAccounts, Accounts>();
        services.AddHttpClient<IPhotoStorage, CloudinaryPhotoStorage>(c => c.Timeout = TimeSpan.FromSeconds(30));
        services.AddHttpClient("resend", c => c.Timeout = TimeSpan.FromSeconds(15));
        services.AddSingleton<IExerciseCache, ExerciseCache>();
        services.AddScoped<MaintenanceJobs>();
        if (configuration.GetValue("Jobs:Enabled", true))
            services.AddHostedService<MaintenanceWorker>();
        return services;
    }
}
