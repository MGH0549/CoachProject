using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coaching.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedIdentityRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Existing installations may already have roles created by coach bootstrap.
            // Match normalized names so their IDs and user assignments are preserved.
            migrationBuilder.Sql("""
                IF NOT EXISTS (SELECT 1 FROM [AspNetRoles] WHERE [NormalizedName] = N'COACH')
                BEGIN
                    INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
                    VALUES (N'coaching-role-coach', N'Coach', N'COACH', N'coaching-role-coach-v1');
                END;

                IF NOT EXISTS (SELECT 1 FROM [AspNetRoles] WHERE [NormalizedName] = N'CLIENT')
                BEGIN
                    INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
                    VALUES (N'coaching-role-client', N'Client', N'CLIENT', N'coaching-role-client-v1');
                END;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF EXISTS (
                    SELECT 1 FROM [AspNetUserRoles]
                    WHERE [RoleId] IN (N'coaching-role-coach', N'coaching-role-client'))
                    THROW 51000, 'Cannot remove seeded roles while users are assigned to them.', 1;

                DELETE FROM [AspNetRoles]
                WHERE ([Id] = N'coaching-role-coach' AND [NormalizedName] = N'COACH')
                   OR ([Id] = N'coaching-role-client' AND [NormalizedName] = N'CLIENT');
                """);
        }
    }
}
