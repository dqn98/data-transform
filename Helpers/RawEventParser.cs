using DataTransform.Models;

namespace DataTransform.Helpers;

public static class RawEventParser
{
    public static RawUserEvent MapToRawUserEvent(this JsonEvent jsonEvent)
    {
        if (jsonEvent == null || jsonEvent.Properties == null)
            throw new ArgumentNullException(nameof(jsonEvent), "JsonEvent or its Properties cannot be null.");

        return new RawUserEvent
        {
            Type = jsonEvent.Type,
            EventName = jsonEvent.Event,
            ProductArea = jsonEvent.Properties.ProductArea,
            ProductValueStream = jsonEvent.Properties.ProductValueStream,
            UiScreenState = jsonEvent.Properties.UiScreenState,
            EventId = jsonEvent.Properties.EventId,
            FlowConversion = jsonEvent.Properties.FlowConversion,
            SessionId = jsonEvent.Properties.SessionId,
            TerminalId = jsonEvent.Properties.TerminalId,
            Amount = decimal.Parse(jsonEvent.Properties.Amount ?? "0"),
            MemberId = int.Parse(jsonEvent.Properties.MemberId ?? "0"),
            CreatedDate = ParseUtcDateTime(jsonEvent.Properties.CreatedDate),
            ProcessedDate = null
        };
    }

    private static DateTime ParseUtcDateTime(string? dateTimeStr)
    {
        if (DateTime.TryParse(dateTimeStr, out var parsedDate))
        {
            return DateTime.SpecifyKind(parsedDate, DateTimeKind.Utc);
        }

        // Fallback to current UTC time
        return DateTime.UtcNow;
    }
}