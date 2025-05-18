using DataTransform.Core.Interfaces;
using DataTransform.Core.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
                var rawEvents = await _rawRepository.GetAllAsync(cancellationToken);
                _logger.LogInformation($"Retrieved {rawEvents.Count()} raw events for processing");
                
                // Transform data
                var processedEvents = new List<UserEvent>();
                
                foreach (var rawEvent in rawEvents)
                {
                    try
                    {
                        var processedEvent = TransformEvent(rawEvent);
                        processedEvents.Add(processedEvent);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error transforming event with ID {rawEvent.Id}");
                    }
                }
                
                // Save processed data
                await _processedRepository.AddRangeAsync(processedEvents, cancellationToken);
                await _processedRepository.SaveChangesAsync(cancellationToken);
                
                _logger.LogInformation($"Successfully transformed and saved {processedEvents.Count} events");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during data transformation process");
                throw;
            }
        }
        
        private UserEvent TransformEvent(RawUserEvent rawEvent)
        {
            // Parse client info to extract app version, device type, and OS version
            var clientInfo = ParseClientInfo(rawEvent.ClientInfo);
            
            // Parse transaction data if available
            var transactionInfo = !string.IsNullOrEmpty(rawEvent.TransactionData) 
                ? ParseTransactionData(rawEvent.TransactionData) 
                : (null, null, null, null);
            
            // Create processed event
            return new UserEvent
            {
                EventId = rawEvent.Id, // Using the raw ID as the event ID
                UserId = rawEvent.UserIdentifier,
                EventName = ExtractEventName(rawEvent.EventDetails),
                EventType = rawEvent.EventType,
                EventTimestamp = rawEvent.Timestamp,
                AppVersion = clientInfo.appVersion,
                DeviceType = clientInfo.deviceType,
                OsVersion = clientInfo.osVersion,
                Location = rawEvent.GeoData,
                PaymentAmount = transactionInfo.amount,
                PaymentStatus = transactionInfo.status,
                TransactionId = transactionInfo.transactionId,
                PaymentMethod = transactionInfo.method
            };
        }
        
        private (string appVersion, string deviceType, string osVersion) ParseClientInfo(string clientInfo)
        {
            try
            {
                // Assuming client info is in JSON format
                var info = JsonConvert.DeserializeObject<Dictionary<string, string>>(clientInfo);
                
                return (
                    info.TryGetValue("app_version", out var appVersion) ? appVersion : "unknown",
                    info.TryGetValue("device_type", out var deviceType) ? deviceType : "unknown",
                    info.TryGetValue("os_version", out var osVersion) ? osVersion : "unknown"
                );
            }
            catch
            {
                return ("unknown", "unknown", "unknown");
            }
        }
        
        private (decimal? amount, string status, string transactionId, string method) ParseTransactionData(string transactionData)
        {
            try
            {
                // Assuming transaction data is in JSON format
                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(transactionData);
                
                decimal? amount = null;
                if (data.TryGetValue("amount", out var amountObj) && decimal.TryParse(amountObj.ToString(), out var parsedAmount))
                {
                    amount = parsedAmount;
                }
                
                return (
                    amount,
                    data.TryGetValue("status", out var status) ? status.ToString() : null,
                    data.TryGetValue("transaction_id", out var transactionId) ? transactionId.ToString() : null,
                    data.TryGetValue("payment_method", out var method) ? method.ToString() : null
                );
            }
            catch
            {
                return (null, null, null, null);
            }
        }
        
        private string ExtractEventName(string eventDetails)
        {
            try
            {
                // Assuming event details is in JSON format and contains an event_name field
                var details = JsonConvert.DeserializeObject<Dictionary<string, string>>(eventDetails);
                
                return details.TryGetValue("event_name", out var eventName) ? eventName : "unknown";
            }
            catch
            {
                return "unknown";
            }
        }
    }
}