using Microsoft.EntityFrameworkCore;
using Coaching.Application;
using Coaching.Domain;

namespace Coaching.Infrastructure.Security;

public sealed partial class Accounts
{
    public async Task<Result<string>> Register(string email, string password, string name, string whatsapp, CancellationToken ct)
    {
        return await db.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
        {
            db.ChangeTracker.Clear();
            await using var tx = await db.Database.BeginTransactionAsync(ct);
            if (await users.FindByEmailAsync(email) is not null)
                return Result<string>.Fail("email_in_use", "البريد الإلكتروني مستخدم بالفعل", 409);
            var normalizedRole = Roles.Client.ToUpperInvariant();
            if (!await db.Roles.AnyAsync(x => x.NormalizedName == normalizedRole, ct))
                return Result<string>.Fail("registration_unavailable", "التسجيل غير متاح مؤقتاً. يرجى التواصل مع المدرب", 503);
            var user = new AppUser
            {
                UserName = email,
                Email = email
            };
            var created = await users.CreateAsync(user, password);
            if (!created.Succeeded)
                return Result<string>.Fail("registration_failed", "تعذر التسجيل. استخدم كلمة مرور قوية وبريداً صالحاً");
            var role = await users.AddToRoleAsync(user, Roles.Client);
            if (!role.Succeeded)
                throw new InvalidOperationException("Client role is not initialized.");
            db.Add(new ClientProfile { ClientId = user.Id, Email = email, FullName = name, WhatsAppNumber = whatsapp, CreatedAt = time.UtcNow, ConsentAt = time.UtcNow });
            await db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
            return Result<string>.Ok(user.Id);
        });
    }
}
