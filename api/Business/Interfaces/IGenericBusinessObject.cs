using Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IGenericBusinessObject
    {
        public Task<T?> GetAsync<T>(Guid uuid) where T : Entity;
        public Task InsertAsync<T>(T record) where T : Entity;
        public Task<List<T>> ListAsync<T>() where T : Entity;
        public Task RemoveAsync<T>(T record, bool isSoftDelete = true) where T : Entity;
        public Task RemoveAsync<T>(Guid uuid, bool isSoftDelete = true) where T : Entity;
        public Task UpdateAsync<T>(T record) where T : Entity;

    }
}
