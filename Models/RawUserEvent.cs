namespace DataTransform.Core.Models
{
    public class RawUserEvent
    {
        public long Id { get; set; }
        public long UserIdentifier { get; set; }
        public string EventType { get; set; } = string.Empty;
        public string EventDetails { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string ClientInfo { get; set; } = string.Empty;
        public string? GeoData { get; set; }
        public string? TransactionData { get; set; }
    }
}