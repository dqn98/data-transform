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
                        SpecTrack = "0.1.0",
                        ProductArea = "payments-and-transaction",
                        ProductValueStream = "tap-payment",
                        UiScreenPath = "/001-value-stream/screen-grouping/path-type/description",
                        UiScreenState = "functional",
                        FlowConversion = "yes-positive",
                        FlowRevenue = 29.99m,
                        UiInputMethod = "keyboard-keypad",
                        DeviceId = "device123",
                        DevSessionId = "sessionABC",
                        DevErrorCode = null,
                        DevErrorDescription = null,
                        DevInputDataCaptured = "{\"amount\":\"29.99\"}",
                        DevSdkVersion = "sdk-v1.0.0",
                        Timestamp = DateTime.UtcNow.AddDays(-1),
                        // CreatedDate is set by the constructor
                        ProcessedDate = null
                    },
                    new RawUserEvent
                    {
                        SpecTrack = "0.1.1",
                        ProductArea = "user-onboarding",
                        ProductValueStream = "registration",
                        UiScreenPath = "/002-value-stream/registration/enter-details",
                        UiScreenState = "validation-error",
                        FlowConversion = "no-negative",
                        FlowRevenue = null, // No revenue for this event
                        UiInputMethod = "touchscreen",
                        DeviceId = "device456",
                        DevSessionId = "sessionXYZ",
                        DevErrorCode = "VAL-001",
                        DevErrorDescription = "Invalid email format",
                        DevInputDataCaptured = "{\"email\":\"test@test\"}",
                        DevSdkVersion = "sdk-v1.0.1",
                        Timestamp = DateTime.UtcNow.AddHours(-5),
                        // CreatedDate is set by the constructor
                        ProcessedDate = null
                    },
                    new RawUserEvent
                    {
                        SpecTrack = "0.2.0",
                        ProductArea = "account-management",
                        ProductValueStream = "profile-update",
                        UiScreenPath = "/003-value-stream/profile/update-address",
                        UiScreenState = "success",
                        FlowConversion = "yes-neutral", // Neutral conversion, e.g., profile update
                        FlowRevenue = null,
                        UiInputMethod = "voice-input",
                        DeviceId = "device789",
                        DevSessionId = "sessionDEF",
                        DevErrorCode = null,
                        DevErrorDescription = null,
                        DevInputDataCaptured = "{\"address\":\"123 Main St\"}",
                        DevSdkVersion = "sdk-v1.1.0",
                        Timestamp = DateTime.UtcNow.AddMinutes(-30),
                        // CreatedDate is set by the constructor
                        ProcessedDate = null
                    }
                    // Add 20 more events for testing
                };

            for (int i = 1; i <= 20; i++)
            {
                rawEvents.Add(new RawUserEvent
                {
                    SpecTrack = $"0.3.{i}",
                    ProductArea = i % 3 == 0 ? "analytics" : (i % 2 == 0 ? "user-engagement" : "feature-discovery"),
                    ProductValueStream = $"test-stream-{i}",
                    UiScreenPath = $"/test/path{i}/screen{i}",
                    UiScreenState = i % 4 == 0 ? "loading" : (i % 3 == 0 ? "error" : (i % 2 == 0 ? "empty" : "populated")),
                    FlowConversion = i % 5 == 0 ? "yes-positive" : (i % 4 == 0 ? "no-negative" : (i % 3 == 0 ? "yes-neutral" : "no-neutral")),
                    FlowRevenue = i % 5 == 0 ? (decimal)(i * 10.5) : (decimal?)null,
                    UiInputMethod = i % 3 == 0 ? "gesture" : (i % 2 == 0 ? "external-device" : "biometric"),
                    DeviceId = $"test-device-{Guid.NewGuid().ToString().Substring(0, 8)}",
                    DevSessionId = $"test-session-{Guid.NewGuid().ToString().Substring(0, 12)}",
                    DevErrorCode = i % 3 == 0 ? $"ERR-{i:000}" : null,
                    DevErrorDescription = i % 3 == 0 ? $"Test error description {i}" : null,
                    DevInputDataCaptured = $"{{\"test_data_{i}\":\"value_{i}\", \"user_action\":\"click_{i}\"}}",
                    DevSdkVersion = $"sdk-v1.2.{i}",
                    Timestamp = DateTime.UtcNow.AddSeconds(-(i * 15)), // Vary timestamps
                                                                       // CreatedDate is set by the constructor
                    ProcessedDate = null
                });
            }

            await context.RawUserEvents.AddRangeAsync(rawEvents, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
