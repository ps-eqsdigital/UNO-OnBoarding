using api.Requests;
using Business.Base;
using Business.BusinessObjects;
using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("Sensor")]

    public class SensorController : Controller
    {
        private readonly IGenericBusinessObject _genericBusinessObject;
        private readonly ISensorBusinessObject _sensorBusinessObject ;
        public SensorController(IGenericBusinessObject genericBusinessObject, ISensorBusinessObject sensorBusinessObject)
        {
            _genericBusinessObject = genericBusinessObject;
            _sensorBusinessObject = sensorBusinessObject;
        }

        [HttpPost("insert")]
        public async Task<ActionResult<Guid>> InsertSensor([FromBody] SensorRequest sensor, string token)
        {
            OperationResult result = await _sensorBusinessObject.CreateSensor(sensor.ToSensor(), token);
            if (result.Exception is Exception)
            {
                return StatusCode(400);
            }
            return Ok(result);
        }
        [HttpPut("update")]
        public async Task<ActionResult> Update(Guid uuid, [FromBody] SensorRequest sensor, string token)
        {
            OperationResult result = await _sensorBusinessObject.EditSensor(uuid, sensor.ToSensor(), token);
            if (result.Exception is Exception)
            {
                return StatusCode(400);
            }
            return StatusCode(200);
        }
    }
}
