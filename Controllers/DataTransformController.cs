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

        public DataTransformController(
            BackgroundJobService jobService,
            ILogger<DataTransformController> logger)
        {
            ArgumentNullException.ThrowIfNull(jobService);
            ArgumentNullException.ThrowIfNull(logger);
            
            _jobService = jobService;
            _logger = logger;
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
            
            if (jsonEvent == null)
            {
                return BadRequest("Invalid JSON payload.");
            }

            // You can add logic here to process or store the event
            return Ok(new { message = "Event received successfully", data = jsonEvent });
        }
    }
}