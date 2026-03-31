using Microsoft.AspNetCore.Mvc;

namespace LoanApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Status = "Healthy",
                Message = "Ocelot API Gateway is up and running.",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}
