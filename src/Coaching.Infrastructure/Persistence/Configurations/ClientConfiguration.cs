using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Coaching.Domain;

namespace Coaching.Infrastructure.Persistence;

public sealed class ClientConfiguration : IEntityTypeConfiguration<ClientProfile>
{
    public void Configure(EntityTypeBuilder<ClientProfile> b)
    {
        b.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
        b.HasIndex(x => x.ClientId).IsUnique();
        b.HasIndex(x => new { x.Status, x.AccessEndsAt });
        b.Property(x => x.Email).HasMaxLength(256);
        b.Property(x => x.FullName).HasMaxLength(200);
        b.Property(x => x.WhatsAppNumber).HasMaxLength(20);
        b.Property(x => x.Package).HasMaxLength(200);
    }
}
