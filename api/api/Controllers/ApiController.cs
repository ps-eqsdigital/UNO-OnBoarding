using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api")]

    public class ApiController : Controller
    {
        private readonly IApiBusinessObject _apiBusinessObject;
        public ApiController(IApiBusinessObject apiBusinessObject) {
            _apiBusinessObject = apiBusinessObject;
        }
        [HttpGet("getVersion")]
        public async Task<ActionResult> GetVersion()
        {
            var result = await _apiBusinessObject.GetApiVersion();

            return Ok(result);
        }
    }
}