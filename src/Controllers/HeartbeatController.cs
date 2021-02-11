using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace payment_gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HeartbeatController : ControllerBase
    {
        private readonly ILogger<HeartbeatController> _logger;

        public HeartbeatController(ILogger<HeartbeatController> logger)
        {
            _logger = logger;

        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok();
        }
    }
}
