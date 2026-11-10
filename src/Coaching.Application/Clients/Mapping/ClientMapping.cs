using Coaching.Domain;

namespace Coaching.Application.Clients;

public static class ClientMapping
{
    public static AccountResponse Response(this ClientProfile x, DateTimeOffset now) => new(x.ClientId!, x.FullName, x.Email, x.WhatsAppNumber, x.Package, x.EffectiveStatus(now), x.AccessEndsAt, x.RowVersion);
}
