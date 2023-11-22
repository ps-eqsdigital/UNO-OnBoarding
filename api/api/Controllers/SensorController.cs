using api.Requests;
using Business.Base;
using Business.BusinessModels;
using Business.BusinessObjects;
using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

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

        [HttpGet("listSensors"), Authorize]
        public async Task<ActionResult<List<SensorBusinessModel>>> ListSensors()
        {
            OperationResult result = await _sensorBusinessObject.ListSensors();
            if (result.Exception is Exception)
            {
                return StatusCode(400);
            }
            return Ok(result);
        }

        [HttpPost("addSensorData"), Authorize]
        public async Task<ActionResult> AddSensorData([FromBody] SensorDataRequest sensorDataRequest)
        {
            OperationResult result = await _sensorBusinessObject.AddData(sensorDataRequest.ToSensorData(), sensorDataRequest.SensorUuid);
            if (result.Exception is Exception)
            {
                return StatusCode(400);
            }
            return StatusCode(200);
        }

        [HttpGet("readData")]
        public async Task<ActionResult> ReadSensorData(Guid sensorUuid, DateTime dateFrom, DateTime dateTo)
        {
            OperationResult result = await _sensorBusinessObject.ReadData(sensorUuid, dateFrom, dateTo);

            if (result.Exception is Exception)
            {
                return StatusCode(400);
            }
            return Ok(result);
        }

        [HttpPost("checkOrUncheckSensorAsFavorite"), Authorize]
        public async Task<ActionResult> CheckOrUncheckSensorAsFavorite([FromBody] MarkFavSensorRequest markFavSensorRequest)
        {
            OperationResult result = await _sensorBusinessObject.MarkOrDemarkSensorAsFavorite(markFavSensorRequest.SensorUuid, markFavSensorRequest.Favorite);

            if (result.Exception is Exception)
            {
                return StatusCode(400);
            }
            return Ok(result);
        }

        [HttpGet("listFavoriteSensors"), Authorize]
        public async Task<ActionResult<List<SensorBusinessModel>>> ListFavoriteSensors()
        {
            OperationResult result = await _sensorBusinessObject.ListFavoriteSensors();
            if (result.Exception is Exception)
            {
                return StatusCode(400);
            }
            return Ok(result);
        }
    }
}
