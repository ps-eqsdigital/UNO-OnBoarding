using Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IGenericDataAccessObject
    {
        Task InsertAsync<T>(T record) where T : Entity;
        Task<List<T>> ListAsync<T>() where T : Entity;
        Task<T?> GetAsync<T>(Guid uuid) where T : Entity;
        Task UpdateAsync<T>(T record) where T : Entity;
        Task RemoveAsync<T>(T record, bool isSoftDelete = true) where T : Entity;
        Task<T?> GetById<T>(long id) where T : Entity;
        Task RemoveAsync<T>(Guid uuid, bool isSoftDelete = true) where T : Entity;

    }
}
