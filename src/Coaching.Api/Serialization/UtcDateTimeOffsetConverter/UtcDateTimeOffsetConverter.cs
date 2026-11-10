using System.Text.Json.Serialization;

namespace Coaching.Api;

public sealed partial class UtcDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
}
