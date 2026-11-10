using System.Security.Cryptography;
using System.Text;

namespace Coaching.Infrastructure.Security;

public sealed partial class Accounts
{
    public static string Hash(string token) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
}
