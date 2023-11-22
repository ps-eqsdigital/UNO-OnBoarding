using Data.Context;
using Data.Entities;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataAccess.DataAccessObjects
{
    public class SensorDataAccessObject : ISensorDataAccessObject
    {
        protected readonly UnoOnBoardingContext _context;

        public SensorDataAccessObject(UnoOnBoardingContext context)
        {
            _context = context;
        }

        public async Task<List<Sensor>> ListSensors(long userId)
        {
            List<Sensor> result = await _context.Set<Sensor>().Where(s => s.IsPublic || s.UserId == userId).ToListAsync();
            return result.Where(x => !x.IsDeleted).ToList()!;
        }

        public async Task<List<SensorData>> ReadData(Guid sensorUuid, DateTime from, DateTime to)
        {
            List<SensorData> result = await _context.Set<Sensor>()
                .Where(s => s.Uuid == sensorUuid)
                .Join(
                    _context.Set<SensorData>(),
                    sensor => sensor.Id,
                    sensorData => sensorData.SensorId,
                    (sensor,sensorData) => new {SensorData = sensorData }
                )
                .Where(joined => joined.SensorData.TimeStamp>= from && joined.SensorData.TimeStamp <= to)
                .Select(joined => joined.SensorData)
                .ToListAsync();

            return result.Where(x => !x.IsDeleted).ToList()!;
        }

        public async Task MarkSensorAsFavorite(long userId, long sensorId)
        {
            UserFavoriteSensor result = new UserFavoriteSensor()
            {
                UserId = userId,
                SensorId = sensorId,
            };

            _context.Set<UserFavoriteSensor>()?.AddAsync(result);
            await _context.SaveChangesAsync();
        }

        public async Task DemarkSensorAsFavorite(long userId, long sensorId)
        {
            UserFavoriteSensor? result = await _context.Set<UserFavoriteSensor>().Where(x=> x.UserId == userId && x.SensorId == sensorId).FirstOrDefaultAsync()!;

            _context.Set<UserFavoriteSensor>().Remove(result!);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckIfSensorIsFavorite(long userId, long sensorId)
        {
            UserFavoriteSensor? result = await _context.Set<UserFavoriteSensor>().Where(x => x.UserId == userId && x.SensorId == sensorId).FirstOrDefaultAsync()!;

            if (result == null) return false;
            return true;
        }

        public async Task<List<Sensor>> ListFavoriteSensors(long userId)
        {
           List<Sensor> result = await _context.Set<UserFavoriteSensor>().Where(x=>x.UserId == userId).
                Join(
                   _context.Set<Sensor>(),
                   userFavoriteSensor => userFavoriteSensor.SensorId,
                   sensor => sensor.Id,
                   (userFavoriteSensor,sensor) => new {Sensor = sensor}
               ).
               Select(joined => joined.Sensor)
               .ToListAsync();

            return result.Where(x => !x.IsDeleted).ToList()!;
        }
    }
}
