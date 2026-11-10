using Coaching.Domain;

namespace Coaching.Application.Clients;

public record AccountResponse(string Id, string FullName, string Email, string WhatsAppNumber, string? Package, ClientStatus Status, DateTimeOffset? AccessEndsAt, byte[] RowVersion);
