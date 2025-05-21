using DataTransform.Interfaces;
using DataTransform.Models;
using DataTransform.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataTransform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataTransformController : ControllerBase
    {
        private readonly BackgroundJobService _jobService;
        private readonly ILogger<DataTransformController> _logger;
        private readonly IEventService _eventService;

        public DataTransformController(
            BackgroundJobService jobService,
            ILogger<DataTransformController> logger,
            IEventService eventService)
        {
            ArgumentNullException.ThrowIfNull(jobService);
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(eventService);
            
            _jobService = jobService;
            _logger = logger;
            _eventService = eventService;
        }

        [HttpPost("trigger")]
        public IActionResult TriggerTransformation()
        {
            var jobId = _jobService.TriggerManualJob();
            return Ok(new { jobId, message = "Data transformation job triggered successfully" });
        }

        [HttpPost]
        public IActionResult AddRawEvent([FromBody] JsonEvent jsonEvent)
        {
            try
            {
                var rawEventId = _eventService.SaveEvent(jsonEvent).Result;
                return Ok(new { message = "Event received successfully", data = rawEventId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Event save failed with message: {ex.Message}", data = jsonEvent });
            }
        }
    }
}