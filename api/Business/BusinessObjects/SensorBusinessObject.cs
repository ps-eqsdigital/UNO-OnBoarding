using Business.Base;
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
        public SensorBusinessObject(IGenericDataAccessObject genericDataAccessObject, IUserDataAccessObject userDataAccessObject, IHttpContextAccessor httpContextAccessor)
        {
            _genericDataAccessObject = genericDataAccessObject;
            _userDataAccessObject = userDataAccessObject;
            _httpContextAccessor = httpContextAccessor;
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
