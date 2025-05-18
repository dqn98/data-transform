using DataTransform.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DataTransform.Infrastructure.Data
{
    public class RawDbContext : DbContext
    {
        public RawDbContext(DbContextOptions<RawDbContext> options) : base(options)
        {
        }

        public DbSet<RawUserEvent> RawUserEvents { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RawUserEvent>().ToTable("raw_user_events");
            
            // Configure entity properties
            modelBuilder.Entity<RawUserEvent>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserIdentifier).HasColumnName("user_identifier");
                entity.Property(e => e.EventType).HasColumnName("event_type").HasMaxLength(100);
                entity.Property(e => e.EventDetails).HasColumnName("event_details").HasMaxLength(255);
                entity.Property(e => e.Timestamp).HasColumnName("timestamp");
                entity.Property(e => e.ClientInfo).HasColumnName("client_info").HasMaxLength(255);
                entity.Property(e => e.GeoData).HasColumnName("geo_data").HasMaxLength(255);
                entity.Property(e => e.TransactionData).HasColumnName("transaction_data").HasMaxLength(255);
            });
        }
    }
}