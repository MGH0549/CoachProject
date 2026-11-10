using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Coaching.Domain;

namespace Coaching.Infrastructure.Persistence;

public sealed class RefreshConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> b)
    {
        b.Property(x => x.Hash).HasMaxLength(64);
        b.HasIndex(x => x.Hash).IsUnique();
        b.HasIndex(x => x.Family);
        b.HasIndex(x => x.ExpiresAt);
    }
}
