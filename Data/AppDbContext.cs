using DataTransform.Core.Models;
using DataTransform.Models;
using Microsoft.EntityFrameworkCore;

namespace DataTransform.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<RawUserEvent> RawUserEvents { get; set; } = null!;
        public DbSet<UserEvent> UserEvents { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure RawUserEvent entity
            modelBuilder.Entity<RawUserEvent>().ToTable("raw_user_events");
            modelBuilder.Entity<RawUserEvent>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityColumn();
            
                // Required fields
                entity.Property(e => e.SpecTrack).HasColumnName("spec_track").HasMaxLength(100).IsRequired();
                entity.Property(e => e.ProductArea).HasColumnName("product_area").HasMaxLength(100).IsRequired();
                entity.Property(e => e.ProductValueStream).HasColumnName("product_value_stream").HasMaxLength(100).IsRequired();
                entity.Property(e => e.UiScreenPath).HasColumnName("ui_screen_path").HasMaxLength(255).IsRequired();
                entity.Property(e => e.UiScreenState).HasColumnName("ui_screen_state").HasMaxLength(100).IsRequired();
                entity.Property(e => e.FlowConversion).HasColumnName("flow_conversion").HasMaxLength(100).IsRequired();
            
                // Conditional field
                entity.Property(e => e.FlowRevenue).HasColumnName("flow_revenue").HasColumnType("decimal(10, 2)");
            
                // Optional fields
                entity.Property(e => e.UiInputMethod).HasColumnName("ui_input_method").HasMaxLength(100);
                entity.Property(e => e.DeviceId).HasColumnName("device_id").HasMaxLength(100);
                entity.Property(e => e.DevSessionId).HasColumnName("dev_session_id").HasMaxLength(100);
                entity.Property(e => e.DevErrorCode).HasColumnName("dev_error_code").HasMaxLength(100);
                entity.Property(e => e.DevErrorDescription).HasColumnName("dev_error_description").HasMaxLength(255);
                entity.Property(e => e.DevInputDataCaptured).HasColumnName("dev_input_data_captured").HasMaxLength(255);
                entity.Property(e => e.DevSdkVersion).HasColumnName("dev_sdk_version").HasMaxLength(100);
            
                // System fields
                entity.Property(e => e.Timestamp).HasColumnName("timestamp");
                entity.Property(e => e.CreatedDate).HasColumnName("created_date");
                entity.Property(e => e.ProcessedDate).HasColumnName("processed_date");
            });

            // Configure UserEvent entity
            modelBuilder.Entity<UserEvent>().ToTable("user_events");
            modelBuilder.Entity<UserEvent>(entity =>
            {
                entity.HasKey(e => e.EventId);
                entity.Property(e => e.EventId)
                    .HasColumnName("id")
                    .UseIdentityColumn();
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.EventName).HasColumnName("event_name").HasMaxLength(255);
                entity.Property(e => e.EventType).HasColumnName("event_type").HasMaxLength(100);
                entity.Property(e => e.EventTimestamp).HasColumnName("event_timestamp");
                entity.Property(e => e.AppVersion).HasColumnName("app_version").HasMaxLength(50);
                entity.Property(e => e.DeviceType).HasColumnName("device_type").HasMaxLength(50);
                entity.Property(e => e.OsVersion).HasColumnName("os_version").HasMaxLength(50);
                entity.Property(e => e.Location).HasColumnName("location").HasMaxLength(255);
                entity.Property(e => e.PaymentAmount).HasColumnName("payment_amount").HasColumnType("decimal(10, 2)");
                entity.Property(e => e.PaymentStatus).HasColumnName("payment_status").HasMaxLength(50);
                entity.Property(e => e.TransactionId).HasColumnName("transaction_id").HasMaxLength(100);
                entity.Property(e => e.PaymentMethod).HasColumnName("payment_method").HasMaxLength(50);
            });
        }
    }
}