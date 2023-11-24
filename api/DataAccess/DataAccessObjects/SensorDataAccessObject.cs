using Data.Context;
using Data.Entities;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
