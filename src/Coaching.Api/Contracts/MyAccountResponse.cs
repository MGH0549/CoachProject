using Coaching.Application.Clients;

namespace Coaching.Api;

public record MyAccountResponse(AccountResponse Account, string WhatsAppLink, string TimeZone, DayOfWeek CheckInDueDay);
