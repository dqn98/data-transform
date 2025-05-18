using DataTransform.Core.Models;
using DataTransform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            var rawDbContext = scope.ServiceProvider.GetRequiredService<RawDbContext>();
            var processedDbContext = scope.ServiceProvider.GetRequiredService<ProcessedDbContext>();
            
            // Ensure databases are created
            await rawDbContext.Database.EnsureCreatedAsync(cancellationToken);
            await processedDbContext.Database.EnsureCreatedAsync(cancellationToken);
            
            // Check if raw database is empty
            if (!await rawDbContext.RawUserEvents.AnyAsync(cancellationToken))
            {
                _logger.LogInformation("Raw database is empty. Seeding initial data...");
                await SeedRawDataAsync(rawDbContext, cancellationToken);
                _logger.LogInformation("Raw database seeded successfully");
            }
            else
            {
                _logger.LogInformation("Raw database already contains data. Skipping seed operation");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping database seeder service");
            return Task.CompletedTask;
        }

        private async Task SeedRawDataAsync(RawDbContext context, CancellationToken cancellationToken)
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
                    GeoData = "New York, USA"
                },
                new RawUserEvent
                {
                    UserIdentifier = 1002,
                    EventType = "purchase",
                    EventDetails = JsonConvert.SerializeObject(new { event_name = "item_purchase", item_id = "SKU12345" }),
                    Timestamp = DateTime.UtcNow.AddHours(-12),
                    ClientInfo = JsonConvert.SerializeObject(new { app_version = "1.1.9", device_type = "tablet", os_version = "Android 12" }),
                    GeoData = "London, UK",
                    TransactionData = JsonConvert.SerializeObject(new { amount = 29.99, status = "completed", transaction_id = "TX789012", payment_method = "credit_card" })
                },
                new RawUserEvent
                {
                    UserIdentifier = 1003,
                    EventType = "registration",
                    EventDetails = JsonConvert.SerializeObject(new { event_name = "new_user_registration", referral = "direct" }),
                    Timestamp = DateTime.UtcNow.AddDays(-2),
                    ClientInfo = JsonConvert.SerializeObject(new { app_version = "1.2.0", device_type = "desktop", os_version = "Windows 11" }),
                    GeoData = "Sydney, Australia"
                },
                new RawUserEvent
                {
                    UserIdentifier = 1001,
                    EventType = "purchase",
                    EventDetails = JsonConvert.SerializeObject(new { event_name = "subscription_purchase", plan = "premium" }),
                    Timestamp = DateTime.UtcNow.AddHours(-6),
                    ClientInfo = JsonConvert.SerializeObject(new { app_version = "1.2.0", device_type = "mobile", os_version = "iOS 15.4" }),
                    GeoData = "New York, USA",
                    TransactionData = JsonConvert.SerializeObject(new { amount = 99.99, status = "completed", transaction_id = "TX789013", payment_method = "paypal" })
                },
                new RawUserEvent
                {
                    UserIdentifier = 1004,
                    EventType = "login",
                    EventDetails = JsonConvert.SerializeObject(new { event_name = "user_login", status = "failed", reason = "invalid_password" }),
                    Timestamp = DateTime.UtcNow.AddHours(-2),
                    ClientInfo = JsonConvert.SerializeObject(new { app_version = "1.1.8", device_type = "mobile", os_version = "Android 11" }),
                    GeoData = "Berlin, Germany"
                }
            };

            await context.RawUserEvents.AddRangeAsync(rawEvents, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}