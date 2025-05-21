using System.ComponentModel.DataAnnotations;

namespace DataTransform.Models
{
    public class UserEvent
    {
        public long EventId { get; set; }
        public long UserId { get; set; }
        
        [MaxLength(20)]
        public string? TerminalId { get; set; }
        
        public long DMemberKey { get; set; }
        
        public long DEftTerminalKey { get; set; }
        
        public long DVehicleTypeKey { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public DateTime EventTimestamp { get; set; }
        public string AppVersion { get; set; } = string.Empty;
        public string DeviceType { get; set; } = string.Empty;
        public decimal? PaymentAmount { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TransactionId { get; set; }
        public string? PaymentMethod { get; set; }
    }
}