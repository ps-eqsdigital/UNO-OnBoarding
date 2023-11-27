using Business.Base;
using Business.BusinessModels;
using Business.Interfaces;
using Data.Entities;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.BusinessObjects
{
    public class SensorBusinessObject : AbstractBusinessObject, ISensorBusinessObject
    {
        private readonly IGenericDataAccessObject _genericDataAccessObject;
        private readonly IUserDataAccessObject _userDataAccessObject;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISensorDataAccessObject _sensorDataAccessObject;

        public SensorBusinessObject(IGenericDataAccessObject genericDataAccessObject, IUserDataAccessObject userDataAccessObject, IHttpContextAccessor httpContextAccessor, ISensorDataAccessObject sensorDataAccessObject)
        {
            _genericDataAccessObject = genericDataAccessObject;
            _userDataAccessObject = userDataAccessObject;
            _httpContextAccessor = httpContextAccessor;
            _sensorDataAccessObject = sensorDataAccessObject;
        }

        public async Task<OperationResult> CreateSensor(Sensor record)
        {
            return await ExecuteOperation(async () =>
            {
                string currentUser = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                if (currentUser == null)
                {
                    throw new Exception("Session expired");
                }

                if (record.Name.IsNullOrEmpty() || record.Description.IsNullOrEmpty() || record.Category.IsNullOrEmpty() || record.Color.IsNullOrEmpty()) {
                    throw new Exception("Missing fields");
                }

                long currentUserId = long.Parse(currentUser);
                record.UserId = currentUserId;
                await _genericDataAccessObject.InsertAsync<Sensor>(record);
            });
        }

        public async Task<OperationResult<List<SensorBusinessModel>>> ListSensors()
        {
            return await ExecuteOperation(async () =>
            {
                string currentUser = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                long currentUserId = long.Parse(currentUser);

                if (currentUser == null)
                {
                    throw new Exception("Session expired");
                }

                List<Sensor> sensors = await _sensorDataAccessObject.ListSensors(currentUserId);
                List<SensorBusinessModel> result = sensors.Select(s => new SensorBusinessModel(s)).ToList();

                List<SensorBusinessModel> result = sensors != null ?
                sensors.Select(s => new SensorBusinessModel(s)).ToList()
                : new List<SensorBusinessModel>();
                return result;
            });
        }

        public async Task<OperationResult> AddData(List<SensorData> sensorData, Guid sensorUuid)
        {
            return await ExecuteOperation(async () =>
            {
                string currentUser = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                long currentUserId = long.Parse(currentUser);

                Sensor? sensor = await _genericDataAccessObject.GetAsync<Sensor>(sensorUuid);

                if (sensor == null)
                {
                    throw new Exception("Sensor doesn't exist");
                }
                
                long sensorId = sensor.Id;

                if (currentUser == null)
                {
                    throw new Exception("Session expired");
                }

                if (currentUserId != sensor.UserId)
                {
                    throw new Exception("Unauthorized");
                }

                foreach(SensorData data in sensorData)
                {
                    data.SensorId= sensorId;
                    await _genericDataAccessObject.InsertAsync(data);
                }
            });
        }

        public async Task<OperationResult<List<List<object>>>> ReadData(Guid sensorUuid, DateTime from, DateTime to)
        {
            return await ExecuteOperation(async () =>
            {
                string currentUser = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                long currentUserId = long.Parse(currentUser);

                if (currentUser == null)
                {
                    throw new Exception("Session expired");
                }

                Sensor? sensor = await _genericDataAccessObject.GetAsync<Sensor>(sensorUuid);
          
                if (sensor == null)
                {
                    throw new Exception("sensor does not exist");
                }

                if (!sensor.IsPublic && currentUserId != sensor.UserId)
                {
                    throw new Exception("Sensor is not accessible");
                }

                List<SensorData> result = await _sensorDataAccessObject.ReadData(currentUserId, sensorUuid, from, to);
                List<List<object>> sensorData = result.Select(data =>
                {
                    ReadDataBusinessModel dataModel = new ReadDataBusinessModel()
                    {
                        TimeStamp = data.TimeStamp,
                        Value = data.Value,
                    };

                    return new List<object>()
                    {
                        dataModel.TimeStamp!,
                        dataModel.Value!
                    };
                }).ToList();

                return sensorData;
            });
        }
         
        public async Task<OperationResult> MarkOrDemarkSensorAsFavorite(Guid sensorUuid, bool favorite)
        {
            return await ExecuteOperation(async () =>
            {

                string currentUser = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                long currentUserId = long.Parse(currentUser);

                if (currentUser == null)
                {
                    throw new Exception("Session expired");
                }

                Sensor? sensor = await _genericDataAccessObject.GetAsync<Sensor>(sensorUuid);

                if (sensor == null)
                {
                    throw new Exception("Invalid sensor");
                }

                if (!sensor.IsPublic && currentUserId != sensor.UserId)
                {
                    throw new Exception("Sensor is not accessible");
                }

                bool isFavoriteSensor = await _sensorDataAccessObject.CheckIfSensorIsFavorite(currentUserId, sensor.Id);

                if (favorite)
                {
                    await _sensorDataAccessObject.MarkSensorAsFavorite(currentUserId, sensor.Id);
                }
                else
                {
                    if (isFavoriteSensor)
                    {
                        await _sensorDataAccessObject.DemarkSensorAsFavorite(currentUserId, sensor.Id);
                    }
                }
            });
        }

        public async Task<OperationResult<List<SensorBusinessModel>>> ListFavoriteSensors()
        {
            return await ExecuteOperation(async () =>
            {
                string currentUser = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                long currentUserId = long.Parse(currentUser);

                if (currentUser == null)
                {
                    throw new Exception("Session expired");
                }

                List<Sensor> sensors = await _sensorDataAccessObject.ListFavoriteSensors(currentUserId);
                List<SensorBusinessModel> result = sensors.Select(s => new SensorBusinessModel(s)).ToList();

                return result;

            });
        }

        public async Task<OperationResult> EditSensor(Guid uuid, Sensor record)
        {
            return await ExecuteOperation(async () =>
            {
                Sensor? sensor = await _genericDataAccessObject.GetAsync<Sensor>(uuid);
                string currentUser = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                long currentUserId= long.Parse(currentUser);

                if (currentUser == null)
                {
                    throw new Exception("Session expired");
                }  
                if (sensor == null)
                {
                    throw new Exception("sensor does not exist");
                }
                if (record.Name.IsNullOrEmpty() || record.Description.IsNullOrEmpty() || record.Category.IsNullOrEmpty() || record.Color.IsNullOrEmpty())
                {
                    throw new Exception("Missing fields");
                }
                if (sensor.UserId != currentUserId)
                {
                    throw new Exception("Unauthorized");
                }
                else
                {
                    sensor.Name = record.Name;
                    sensor.Description = record.Description;
                    sensor.Color = record.Color;
                    sensor.Category = record.Category;
                    sensor.IsPublic = record.IsPublic;
                }

                await _genericDataAccessObject.UpdateAsync<Sensor>(sensor);
            });
        }
    }
}
