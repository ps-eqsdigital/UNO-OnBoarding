using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface ISensorDataAccessObject
    {
        public Task<List<Sensor>> ListSensors(long userId);
        public Task<List<SensorData>> ReadData(long userId, Guid sensorUuid, DateTime from, DateTime to);
        public Task MarkSensorAsFavorite(long userId, long sensorId);
        public Task DemarkSensorAsFavorite(long userId, long sensorId);
        public Task<bool> CheckIfSensorIsFavorite(long userId, long sensorId);
        public Task<List<Sensor>> ListFavoriteSensors(long userId);

    }
}
