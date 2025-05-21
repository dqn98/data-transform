using DataTransform.Interfaces;
using DataTransform.Models;
using Microsoft.EntityFrameworkCore;

namespace DataTransform.Services
{
    public class DataTransformService : IDataTransformService
    {
        private readonly IDataRepository<RawUserEvent> _rawRepository;
        private readonly IDataRepository<UserEvent> _processedRepository;
        private readonly ILogger<DataTransformService> _logger;

        public DataTransformService(
            IDataRepository<RawUserEvent> rawRepository,
            IDataRepository<UserEvent> processedRepository,
            ILogger<DataTransformService> logger)
        {
            ArgumentNullException.ThrowIfNull(rawRepository);
            ArgumentNullException.ThrowIfNull(processedRepository);
            ArgumentNullException.ThrowIfNull(logger);

            _rawRepository = rawRepository;
            _processedRepository = processedRepository;
            _logger = logger;
        }

        public async Task TransformDataAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Starting data transformation process");

                // Get raw data
                var rawEvents = await _rawRepository
                    .FindAsync(e => e.ProcessedDate == null)
                    .ToListAsync(cancellationToken);
                
                _logger.LogInformation($"Retrieved {rawEvents.Count} raw events for processing");

                if (!rawEvents.Any())
                {
                    _logger.LogInformation("No new raw events to process.");
                    return;
                }

                // Transform data
                var processedEvents = new List<UserEvent>();
                var rawEventsToUpdate = new List<RawUserEvent>();

                foreach (var rawEvent in rawEvents)
                {
                    try
                    {
                        var processedEvent = TransformEvent(rawEvent);
                        
                        processedEvent.DEftTerminalKey = GetDummyLong();
                        processedEvent.DVehicleTypeKey = GetDummyLong();
                        processedEvent.DMemberKey = GetDummyLong();
                        
                        processedEvents.Add(processedEvent);
                        
                        rawEvent.ProcessedDate = DateTime.UtcNow; // Use UTC time
                        rawEventsToUpdate.Add(rawEvent);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error transforming event with ID {rawEvent.Id}. This event will not be marked as processed.");
                    }
                }

                // Save processed data
                if (processedEvents.Any())
                {
                    await _processedRepository.AddRangeAsync(processedEvents, cancellationToken);
                    await _processedRepository.SaveChangesAsync(cancellationToken); // Save processed events
                    _logger.LogInformation($"Successfully transformed and saved {processedEvents.Count} events.");
                }
                else
                {
                    _logger.LogInformation("No events were successfully transformed.");
                }

                // Update ProcessedDate for successfully transformed raw events
                if (rawEventsToUpdate.Any())
                {
                    _rawRepository.UpdateRange(rawEventsToUpdate); // This method should handle setting UTC for ProcessedDate
                    await _rawRepository.SaveChangesAsync(cancellationToken); // Save changes to raw events
                    _logger.LogInformation($"Successfully updated ProcessedDate for {rawEventsToUpdate.Count} raw events.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during data transformation process");
            }
        }

        private UserEvent TransformEvent(RawUserEvent rawEvent)
        {
            // Create processed event using new RawUserEvent fields
            return new UserEvent
            {
                // EventId is database-generated
                UserId = rawEvent.MemberId,
                EventName = DetermineEventName(rawEvent.EventName),
                EventType = rawEvent.Type ?? "transaction",
                EventTimestamp = DateTime.SpecifyKind(rawEvent.CreatedDate, DateTimeKind.Utc), // Ensure UTC
                DeviceType = "Iphone",
                PaymentAmount = rawEvent.Amount, // Direct mapping
                PaymentStatus = DeterminePaymentStatus(rawEvent.UiScreenState),
                TransactionId = null, 
                PaymentMethod = "TAP"
            };
        }

        private long ExtractUserId(string? deviceId, string? devSessionId)
        {
            // Prioritize DevSessionId if it can be parsed as a long
            if (!string.IsNullOrEmpty(devSessionId) && long.TryParse(devSessionId, out var userIdFromSession))
            {
                return userIdFromSession;
            }
            // Fallback to a hash of DeviceId or a default value if DeviceId is null/empty
            if (!string.IsNullOrEmpty(deviceId))
            {
                // Example: simple hash. Replace with a more robust hashing or lookup if needed.
                return deviceId.GetHashCode(); 
            }
            // Default or error case
            _logger.LogWarning("Could not determine UserId from DeviceId or DevSessionId.");
            return 0; // Or throw an exception, or assign a specific "unknown user" ID
        }

        private string DetermineEventName(string? uiScreenPath)
        {
            if (string.IsNullOrWhiteSpace(uiScreenPath))
            {
                return "unknown_event";
            }

            Constants.Events.ParsedEventMap.TryGetValue(uiScreenPath, out var parsed);
            
            return string.IsNullOrEmpty(parsed)
                ? "unknown_event"
                : parsed;
        }

        private string? ExtractDeviceType(string? deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                return "unknown";
            }
            // Example: if deviceId is "mobile-android-123xyz", extract "mobile-android"
            // This is a placeholder; adjust based on your actual deviceId format
            var parts = deviceId.Split('-');
            if (parts.Length > 1)
            {
                return string.Join("-", parts.Take(Math.Min(parts.Length, 2))); // Takes first two parts e.g. "type-subtype"
            }
            return deviceId; // Or "unknown" if format is not as expected
        }

        private string DeterminePaymentStatus(string? screen)
        {
            return screen switch
            {
                Constants.Events.PntTnxPaymentSuccessfulScreenViewed => "success",
                Constants.Events.PntTnxPaymentErrorScreenViewed => "failed",
                Constants.Events.PntTnxPaymentDeclineScreenViewed => "declined",
                _ => "unknown"
            };
        }

        private long GetDummyLong()
        {
            var random = new Random();
            byte[] buffer = new byte[8];
            random.NextBytes(buffer);
            
            return BitConverter.ToInt64(buffer, 0);
        }
    }
}