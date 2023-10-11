using Business.Interfaces;
using Data.Base;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Base
{
    public class GenericBusinessObject : IGenericBusinessObject
    {
        private readonly IGenericDataAccessObject _genericDataAccessObject;


        public GenericBusinessObject(IGenericDataAccessObject dataAccessObject)
        {
            _genericDataAccessObject = dataAccessObject;
        }

        public async Task<T?> GetAsync<T>(Guid uuid) where T : Entity
        {
             return await _genericDataAccessObject.GetAsync<T>(uuid);
        }


        public async Task InsertAsync<T>(T record) where T : Entity
        {
            await _genericDataAccessObject.InsertAsync(record);
        }


        public async Task<List<T>> ListAsync<T>() where T : Entity
        {
            return await _genericDataAccessObject.ListAsync<T>();
        }


        public async Task RemoveAsync<T>(T record, bool isSoftDelete = true) where T : Entity
        {

            await _genericDataAccessObject.RemoveAsync(record,isSoftDelete);
        }

        public async Task RemoveAsync<T>(Guid uuid,bool isSoftDelete = true) where T : Entity
        {
            await _genericDataAccessObject.RemoveAsync<T>(uuid,isSoftDelete);
        }

        public async Task UpdateAsync<T>(T record) where T : Entity
        {
            await _genericDataAccessObject.UpdateAsync(record);
        }

    }
}
