using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransform.Models
{
    public class RawUserEvent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(200)]
        public string? Type { get; set; }
        [MaxLength(200)]
        public string? EventName { get; set; }
        public string? ProductArea { get; set; }
        public string? ProductValueStream { get; set; }
        public string? UiScreenState { get; set; }
        [MaxLength(200)]
        public string? EventId { get; set; }
        public string? FlowConversion { get; set; }
        [MaxLength(200)]
        public string? SessionId { get; set; }
        public string? TerminalId { get; set; }
        public decimal Amount { get; set; }
        public int MemberId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
    }
}