using System.ComponentModel.DataAnnotations.Schema;

namespace DataTransform.Models
{
    public class RawUserEvent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        // Required fields from the table
        public string SpecTrack { get; set; } = string.Empty; // "spec_track" - Required
        public string ProductArea { get; set; } = string.Empty; // "product_area" - Required
        public string ProductValueStream { get; set; } = string.Empty; // "product_value_stream" - Required
        public string UiScreenPath { get; set; } = string.Empty; // "ui_screen_path" - Required
        public string UiScreenState { get; set; } = string.Empty; // "ui_screen_state" - Required
        public string FlowConversion { get; set; } = string.Empty; // "flow_conversion" - Required
        
        // Conditional field
        public decimal? FlowRevenue { get; set; } // "flow_revenue" - Conditional
        
        // Optional fields
        public string? UiInputMethod { get; set; } // "ui_input_method" - Optional
        public string? DeviceId { get; set; } // "device_id" - Optional
        public string? DevSessionId { get; set; } // "dev_session_id" - Optional
        public string? DevErrorCode { get; set; } // "dev_error_code" - Optional
        public string? DevErrorDescription { get; set; } // "dev_error_description" - Optional
        public string? DevInputDataCaptured { get; set; } // "dev_input_data_captured" - Optional
        public string? DevSdkVersion { get; set; } // "dev_sdk_version" - Optional
        
        // System fields for tracking
        public DateTime Timestamp { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        
        public long TerminalId { get; set; }
        
        // Constructor to set default values
        public RawUserEvent()
        {
            CreatedDate = DateTime.UtcNow; // Use UTC time
        }
    }
}