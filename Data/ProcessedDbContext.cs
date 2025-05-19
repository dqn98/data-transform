using DataTransform.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DataTransform.Data
{
    public class ProcessedDbContext(DbContextOptions<ProcessedDbContext> options) : DbContext(options)
    {
        public DbSet<UserEvent> UserEvents { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEvent>().ToTable("user_events");
            
            // Configure entity properties based on the provided schema
            modelBuilder.Entity<UserEvent>(entity =>
            {
                entity.HasKey(e => e.EventId);
                entity.Property(e => e.EventId).HasColumnName("event_id");
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