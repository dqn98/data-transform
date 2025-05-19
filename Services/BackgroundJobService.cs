using DataTransform.Core.Interfaces;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace DataTransform.Services
{
    public class BackgroundJobService
    {
        private readonly IDataTransformService _dataTransformService;
        private readonly ILogger<BackgroundJobService> _logger;

        public BackgroundJobService(
            IDataTransformService dataTransformService,
            ILogger<BackgroundJobService> logger)
        {
            ArgumentNullException.ThrowIfNull(dataTransformService);
            ArgumentNullException.ThrowIfNull(logger);
            
            _dataTransformService = dataTransformService;
            _logger = logger;
        }

        public void ScheduleRecurringJobs()
        {
            _logger.LogInformation("Scheduling recurring data transformation job");
            
            // Schedule job to run every 30 minutes
            RecurringJob.AddOrUpdate(
                "data-transformation-job",
                () => _dataTransformService.TransformDataAsync(CancellationToken.None),
                "*/1 * * * *" // Cron expression for every 30 minutes
            );
            
            _logger.LogInformation("Data transformation job scheduled successfully");
        }

        public string TriggerManualJob()
        {
            _logger.LogInformation("Triggering manual data transformation job");
            
            // Enqueue a background job to be executed immediately
            var jobId = BackgroundJob.Enqueue(() => _dataTransformService.TransformDataAsync(CancellationToken.None));
            
            _logger.LogInformation($"Manual job triggered with ID: {jobId}");
            
            return jobId;
        }
    }
}