using Microsoft.AspNetCore.Mvc;

namespace ProductService.Controller
{
    [ApiController]
    [Route("health")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(new { status = "Healthy", service = "ProductService", timestamp = DateTime.UtcNow });

    }
}
