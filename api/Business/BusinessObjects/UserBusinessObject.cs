using Business.Base;
using Business.Interfaces;
using Data.Entities;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.BusinessObjects
{
    public class UserBusinessObject : AbstractBusinessObject, IUserBusinessObject
    {
        private readonly IGenericDataAccessObject _genericDataAccessObject;

        public UserBusinessObject(IGenericDataAccessObject genericDataAccessObject) {
            _genericDataAccessObject = genericDataAccessObject;
        }

        public async Task<OperationResult> Update(Guid uuid, User record)
        {
            return await ExecuteOperation(async () =>
            {
                var user = await _genericDataAccessObject.GetAsync<User>(uuid);

                if (user == null)
                {
                    throw new Exception("user doesn't exist");
                }
                else
                {
                    user.Name = record.Name;
                    user.Email = record.Email;
                    user.Picture = record.Picture;
                    user.Password = record.Password;
                    user.Phone = record.Phone;
                    user.Role = record.Role;
                }
                await _genericDataAccessObject.UpdateAsync<User>(user);
            });
        }
    }
}
