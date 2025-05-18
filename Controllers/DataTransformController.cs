using DataTransform.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataTransform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataTransformController : ControllerBase
    {
        private readonly BackgroundJobService _jobService;

        public DataTransformController(BackgroundJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpPost("trigger")]
        public IActionResult TriggerTransformation()
        {
            var jobId = _jobService.TriggerManualJob();
            return Ok(new { jobId, message = "Data transformation job triggered successfully" });
        }
    }
}