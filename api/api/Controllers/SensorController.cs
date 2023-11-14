using api.Requests;
using Business.Base;
using Business.BusinessObjects;
using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("insert"), Authorize]
        public async Task<ActionResult<Guid>> InsertSensor([FromBody] SensorRequest sensor)
        {
            OperationResult result = await _sensorBusinessObject.CreateSensor(sensor.ToSensor());
            if (result.Exception is Exception)
            {
                return StatusCode(400);
            }
            return Ok(result);
        }

        [HttpPut("update"), Authorize]
        public async Task<ActionResult> Update(Guid uuid, [FromBody] SensorRequest sensor)
        {
            OperationResult result = await _sensorBusinessObject.EditSensor(uuid, sensor.ToSensor());
            if (result.Exception is Exception)
            {
                return StatusCode(400);
            }
            return StatusCode(200);
        }
    }
}
