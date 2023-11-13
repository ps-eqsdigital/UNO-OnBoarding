using Business.Base;
using Business.Interfaces;
using Data.Entities;
using DataAccess.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.BusinessObjects
{
    public class SensorBusinessObject : AbstractBusinessObject, ISensorBusinessObject
    {
        private readonly IGenericDataAccessObject _genericDataAccessObject;
        private readonly IUserDataAccessObject _userDataAccessObject;

        public SensorBusinessObject(IGenericDataAccessObject genericDataAccessObject, IUserDataAccessObject userDataAccessObject)
        {
            _genericDataAccessObject = genericDataAccessObject;
            _userDataAccessObject = userDataAccessObject;
        }

        public async Task<OperationResult> CreateSensor(Sensor record, string token)
        {
            return await ExecuteOperation(async () =>
            {
                if (record.Name.IsNullOrEmpty() || record.Description.IsNullOrEmpty() || record.Category.IsNullOrEmpty() || record.Color.IsNullOrEmpty()) {
                    throw new Exception("Missing fields");
                }

                UserTokenAuthentication userToken = await _userDataAccessObject.GetToken(token);
                if (userToken == null)
                {
                    throw new Exception("Sesson expired");
                }

                long? userId = userToken.UserId;
                record.UserId = userId;
                await _genericDataAccessObject.InsertAsync<Sensor>(record);
            });
        }

        public async Task<OperationResult> EditSensor(Guid uuid, Sensor record, string token)
        {
            return await ExecuteOperation(async () =>
            {

                Sensor? sensor = await _genericDataAccessObject.GetAsync<Sensor>(uuid);

                if (sensor == null)
                {
                    throw new Exception("sensor does not exist");
                }
                if (record.Name.IsNullOrEmpty() || record.Description.IsNullOrEmpty() || record.Category.IsNullOrEmpty() || record.Color.IsNullOrEmpty())
                {
                    throw new Exception("Missing fields");
                }
                UserTokenAuthentication userToken = await _userDataAccessObject.GetToken(token);
                if (userToken == null || sensor.UserId != userToken.UserId)
                {
                    throw new Exception("Sesson expired");
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
