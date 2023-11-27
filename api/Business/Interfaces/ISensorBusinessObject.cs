using Business.Base;
using Business.BusinessModels;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ISensorBusinessObject
    {
        public Task<OperationResult> CreateSensor(Sensor record);
        public Task<OperationResult> EditSensor(Guid uuid, Sensor record);
        public Task<OperationResult<List<SensorBusinessModel>>> ListSensors();
        public Task<OperationResult> AddData(List<SensorData> sensorData, Guid sensorUuid);
        public Task<OperationResult<List<List<object>>>> ReadData(Guid sensorUuid, DateTime from, DateTime to);
        public Task<OperationResult> MarkOrDemarkSensorAsFavorite(Guid sensorUuid, bool favorite);
        public Task<OperationResult<List<SensorBusinessModel>>> ListFavoriteSensors();


    }
}
