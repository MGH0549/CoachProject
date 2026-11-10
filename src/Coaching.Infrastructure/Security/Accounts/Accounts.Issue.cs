using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Coaching.Application;
using Coaching.Domain;

namespace Coaching.Infrastructure.Security;

public sealed partial class Accounts
{
    private async Task<Session> Issue(AppUser user, Guid family, DateTimeOffset refreshExpiry)
    {
        var raw = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        db.Add(new RefreshToken { UserId = user.Id, Family = family, Hash = Hash(raw), SecurityStamp = user.SecurityStamp ?? "", ExpiresAt = refreshExpiry });
        var now = time.UtcNow;
        var expiry = now.AddMinutes(jwt.Value.AccessMinutes);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("stamp", user.SecurityStamp ?? "")
        };
        claims.AddRange((await users.GetRolesAsync(user)).Select(r => new Claim(ClaimTypes.Role, r)));
        var token = new JwtSecurityToken(jwt.Value.Issuer, jwt.Value.Audience, claims, now.UtcDateTime, expiry.UtcDateTime, new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Value.Key)), SecurityAlgorithms.HmacSha256));
        return new(new JwtSecurityTokenHandler().WriteToken(token), expiry, raw, refreshExpiry);
    }
}
