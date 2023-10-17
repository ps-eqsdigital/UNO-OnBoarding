using Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api")]

    public class ApiController : Controller
    {
        [HttpGet("getVersion")]
        public async Task<ActionResult> GetVersion()
        {
            var versionInfo = new
            {
                version = "1.0.0",
                timestamp = DateTime.UtcNow.ToString("o")
            };

            return Ok(versionInfo);
        }
    }
}
