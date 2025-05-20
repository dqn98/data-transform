using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using DataTransform.Data;
using DataTransform.Models;

namespace DataTransform.Services
{
    public class DatabaseSeederService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DatabaseSeederService> _logger;

        public DatabaseSeederService(
            IServiceProvider serviceProvider,
            ILogger<DatabaseSeederService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting database seeder service");

            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Check if database exists, if not create it
            await dbContext.Database.EnsureCreatedAsync(cancellationToken);

            // Check if there's any data in the raw_user_events table
            if (!await dbContext.RawUserEvents.AnyAsync(cancellationToken))
            {
                _logger.LogInformation("Seeding raw user events data");
                await SeedRawDataAsync(dbContext, cancellationToken);
                _logger.LogInformation("Raw user events data seeded successfully");
            }
            else
            {
                _logger.LogInformation("Raw user events data already exists, skipping seeding");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping database seeder service");
            return Task.CompletedTask;
        }

        private async Task SeedRawDataAsync(AppDbContext context, CancellationToken cancellationToken)
        {
            var rawEvents = new List<RawUserEvent>
            {
                new RawUserEvent
                {
                    UserIdentifier = 1001,
                    EventType = "login",
                    EventDetails = JsonConvert.SerializeObject(new { event_name = "user_login", status = "success" }),
                    Timestamp = DateTime.UtcNow.AddDays(-1),
                    ClientInfo = JsonConvert.SerializeObject(new { app_version = "1.2.0", device_type = "mobile", os_version = "iOS 15.4" }),
                    GeoData = "New York, USA",
                    CreatedDate = DateTime.UtcNow.AddMinutes(-2), // Use UTC time
                    ProcessedDate = null
                },
                new RawUserEvent
                {
                    UserIdentifier = 1002,
                    EventType = "purchase",
                    EventDetails = JsonConvert.SerializeObject(new { event_name = "item_purchase", item_id = "SKU12345" }),
                    Timestamp = DateTime.UtcNow.AddHours(-12),
                    ClientInfo = JsonConvert.SerializeObject(new { app_version = "1.1.9", device_type = "tablet", os_version = "Android 12" }),
                    GeoData = "London, UK",
                    TransactionData = JsonConvert.SerializeObject(new { amount = 29.99, status = "completed", transaction_id = "TX789012", payment_method = "credit_card" }),
                    CreatedDate = DateTime.UtcNow.AddMinutes(-10), // Use UTC time
                    ProcessedDate = null
                },
                new RawUserEvent
                {
                    UserIdentifier = 1003,
                    EventType = "registration",
                    EventDetails = JsonConvert.SerializeObject(new { event_name = "new_user_registration", referral = "direct" }),
                    Timestamp = DateTime.UtcNow.AddDays(-2),
                    ClientInfo = JsonConvert.SerializeObject(new { app_version = "1.2.0", device_type = "desktop", os_version = "Windows 11" }),
                    GeoData = "Sydney, Australia",
                    CreatedDate = DateTime.UtcNow.AddMinutes(-11), // Use UTC time
                    ProcessedDate = null
                },
                new RawUserEvent
                {
                    UserIdentifier = 1001,
                    EventType = "purchase",
                    EventDetails = JsonConvert.SerializeObject(new { event_name = "subscription_purchase", plan = "premium" }),
                    Timestamp = DateTime.UtcNow.AddHours(-6),
                    ClientInfo = JsonConvert.SerializeObject(new { app_version = "1.2.0", device_type = "mobile", os_version = "iOS 15.4" }),
                    GeoData = "New York, USA",
                    TransactionData = JsonConvert.SerializeObject(new { amount = 99.99, status = "completed", transaction_id = "TX789013", payment_method = "paypal" }),
                    CreatedDate = DateTime.UtcNow.AddDays(-1),
                    ProcessedDate = null
                },
                new RawUserEvent
                {
                    UserIdentifier = 1004,
                    EventType = "login",
                    EventDetails = JsonConvert.SerializeObject(new { event_name = "user_login", status = "failed", reason = "invalid_password" }),
                    Timestamp = DateTime.UtcNow.AddHours(-2),
                    ClientInfo = JsonConvert.SerializeObject(new { app_version = "1.1.8", device_type = "mobile", os_version = "Android 11" }),
                    GeoData = "Berlin, Germany",
                    CreatedDate = DateTime.UtcNow.AddDays(-1),
                    ProcessedDate = null
                }
            };

            await context.RawUserEvents.AddRangeAsync(rawEvents, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}