using DataTransform.Models;
using Microsoft.EntityFrameworkCore;

namespace DataTransform.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<RawUserEvent> RawUserEvents { get; set; } = null!;
        public DbSet<UserEvent> UserEvents { get; set; } = null!;
        public DbSet<TerminalData> TerminalData { get; set; }

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
                
                entity.Property(e => e.Type)
                    .HasMaxLength(200)
                    .HasColumnName("type");

                entity.Property(e => e.EventName)
                    .HasMaxLength(200)
                    .HasColumnName("event_name");

                entity.Property(e => e.ProductArea)
                    .HasColumnName("product_area");

                entity.Property(e => e.ProductValueStream)
                    .HasColumnName("product_value_stream");

                entity.Property(e => e.UiScreenState)
                    .HasColumnName("ui_screen_state");

                entity.Property(e => e.EventId)
                    .HasMaxLength(200)
                    .HasColumnName("event_id");

                entity.Property(e => e.FlowConversion)
                    .HasColumnName("flow_conversion");

                entity.Property(e => e.SessionId)
                    .HasMaxLength(200)
                    .HasColumnName("session_id");

                entity.Property(e => e.TerminalId)
                    .HasColumnName("terminal_id");

                entity.Property(e => e.Amount)
                    .HasColumnName("amount");

                entity.Property(e => e.MemberId)
                    .HasColumnName("member_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ProcessedDate)
                    .HasColumnName("processed_date");


                entity.Property(e => e.TerminalId).HasColumnName("terminal_id").HasMaxLength(20);
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
                entity.Property(e => e.PaymentAmount).HasColumnName("payment_amount").HasColumnType("decimal(10, 2)");
                entity.Property(e => e.PaymentStatus).HasColumnName("payment_status").HasMaxLength(50);
                entity.Property(e => e.TransactionId).HasColumnName("transaction_id").HasMaxLength(100);
                entity.Property(e => e.PaymentMethod).HasColumnName("payment_method").HasMaxLength(50);
                
                entity.Property(e => e.TerminalId).HasColumnName("terminal_id").HasMaxLength(20);
                entity.Property(e => e.DMemberKey).HasColumnName("d_memberkey");
                entity.Property(e => e.DEftTerminalKey).HasColumnName("d_eftterminalkey");
                entity.Property(e => e.DVehicleTypeKey).HasColumnName("d_vehicletypekey");
            });
        
            // Configure TerminalData entity
            modelBuilder.Entity<TerminalData>()
                .ToTable("terminal_data");
                
            modelBuilder.Entity<TerminalData>()
                .Property(t => t.TerminalId)
                .HasColumnName("terminalid");
                
            modelBuilder.Entity<TerminalData>()
                .Property(t => t.D_EftterminalKey)
                .HasColumnName("D_EftterminalKey");
                
            modelBuilder.Entity<TerminalData>()
                .Property(t => t.D_VehicleTypeKey)
                .HasColumnName("D_VehicleTypeKey");
                
            modelBuilder.Entity<TerminalData>()
                .Property(t => t.D_MemberKey)
                .HasColumnName("D_MemberKey");
        }
    }
}