using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransform.Models
{
    public class RawUserEvent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long UserIdentifier { get; set; }
        public string EventType { get; set; } = string.Empty;
        public string EventDetails { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string ClientInfo { get; set; } = string.Empty;
        public string? GeoData { get; set; }
        public string? TransactionData { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public DateTime? ProcessedDate { get; set; }
        
        // When setting CreatedDate
        public RawUserEvent()
        {
            CreatedDate = DateTime.UtcNow; // Use UTC time
        }
    }
}