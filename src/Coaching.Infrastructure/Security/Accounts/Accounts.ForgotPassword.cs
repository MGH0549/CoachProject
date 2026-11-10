using System.Net;
using Coaching.Domain;
using Microsoft.AspNetCore.DataProtection;

namespace Coaching.Infrastructure.Security;

public sealed partial class Accounts
{
    public async Task ForgotPassword(string email, CancellationToken ct)
    {
        var user = await users.FindByEmailAsync(email);
        if (user is null)
            return;
        var token = await users.GeneratePasswordResetTokenAsync(user);
        var url = platform.Value.AngularOrigin.TrimEnd('/') + "/reset-password?email=" + Uri.EscapeDataString(email) + "&token=" + Uri.EscapeDataString(token);
        var html = $"<html lang=\"ar\" dir=\"rtl\"><body><p>لإعادة تعيين كلمة المرور اضغط على الرابط التالي:</p><a href=\"{WebUtility.HtmlEncode(url)}\">إعادة تعيين كلمة المرور</a><p>إذا لم تطلب ذلك، تجاهل هذه الرسالة.</p></body></html>";
        db.Add(new EmailMessage { ClientId = user.Id, Recipient = email, Subject = "إعادة تعيين كلمة المرور", Html = protection.CreateProtector("email-outbox").Protect(html), CreatedAt = time.UtcNow, NextAttemptAt = time.UtcNow });
        await db.SaveChangesAsync(ct);
    }
}
