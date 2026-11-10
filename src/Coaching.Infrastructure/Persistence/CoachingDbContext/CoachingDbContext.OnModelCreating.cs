using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Coaching.Domain;
using Coaching.Infrastructure.Security;

namespace Coaching.Infrastructure.Persistence;

public sealed partial class CoachingDbContext
{
    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);
        foreach (var type in typeof(Entity).Assembly.GetTypes().Where(t => !t.IsAbstract && typeof(Entity).IsAssignableFrom(t)))
        {
            var e = b.Entity(type);
            e.HasBaseType((Type?)null);
            e.HasKey(nameof(Entity.Id));
            e.Property(nameof(Entity.RowVersion)).IsRowVersion();
            foreach (var prop in type.GetProperties().Where(p => p.PropertyType == typeof(string)))
                e.Property(prop.Name).IsUnicode().HasMaxLength(prop.Name.EndsWith("Id", StringComparison.Ordinal) ? 450 : 4000);
            foreach (var prop in type.GetProperties().Where(p => p.PropertyType == typeof(decimal) || p.PropertyType == typeof(decimal?)))
                e.Property(prop.Name).HasPrecision(10, 2);
            if (typeof(ClientEntity).IsAssignableFrom(type))
            {
                var parameter = Expression.Parameter(type, "record");
                var body = Expression.OrElse(Expression.Property(Expression.Constant(this), nameof(CanSeeAll)), Expression.AndAlso(Expression.NotEqual(Expression.Property(Expression.Constant(this), nameof(CurrentUserId)), Expression.Constant(null, typeof(string))), Expression.Equal(Expression.Property(parameter, nameof(ClientEntity.ClientId)), Expression.Property(Expression.Constant(this), nameof(CurrentUserId)))));
                e.HasQueryFilter(Expression.Lambda(body, parameter));
                e.HasIndex(nameof(ClientEntity.ClientId));
            }
        }

        b.ApplyConfigurationsFromAssembly(typeof(CoachingDbContext).Assembly);
        b.Entity<ClientProfile>().HasOne<AppUser>().WithOne().HasForeignKey<ClientProfile>(x => x.ClientId).OnDelete(DeleteBehavior.Cascade);
        b.Entity<ClientProfile>().Property(x => x.ClientId).IsRequired();
        b.Entity<RefreshToken>().HasOne<AppUser>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        b.Entity<EmailMessage>().Property(x => x.Html).HasMaxLength(16000);
        b.Entity<EmailMessage>().HasIndex(x => x.NextAttemptAt);
        b.Entity<AuditEntry>().HasIndex(x => x.At);
        b.Entity<AuditEntry>().HasIndex(x => x.SubjectClientId);
        b.Entity<JobState>().Property(x => x.Name).HasMaxLength(100);
        b.Entity<JobState>().HasIndex(x => x.Name).IsUnique();
        b.Entity<TrainingPlan>().HasOne<AppUser>().WithMany().HasForeignKey(x => x.ClientId).OnDelete(DeleteBehavior.Restrict);
        b.Entity<DietPlan>().HasOne<AppUser>().WithMany().HasForeignKey(x => x.ClientId).OnDelete(DeleteBehavior.Restrict);
        b.Entity<CheckIn>().HasOne<AppUser>().WithMany().HasForeignKey(x => x.ClientId).OnDelete(DeleteBehavior.Restrict);
        b.Entity<WorkoutLog>().HasOne<AppUser>().WithMany().HasForeignKey(x => x.ClientId).OnDelete(DeleteBehavior.Restrict);
        b.Entity<Exercise>().Property(x => x.Name).HasMaxLength(200);
        b.Entity<Exercise>().Property(x => x.MuscleGroup).HasMaxLength(100);
        b.Entity<Exercise>().Property(x => x.YouTubeVideoId).HasMaxLength(11);
        b.Entity<Exercise>().HasIndex(x => new { x.Archived, x.Name });
    }
}
