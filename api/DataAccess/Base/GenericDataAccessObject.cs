using Data.Base;
using Data.Context;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Base
{
    public class GenericDataAccessObject : IGenericDataAccessObject
    {
        protected readonly UnoOnBoardingContext _context;

        
        public GenericDataAccessObject(UnoOnBoardingContext context)
        {
            _context = context;
        }


        public async Task<T?> GetAsync<T>(Guid uuid) where T : Entity
        {
            var result = await _context.Set<T>().Where(x => x.Uuid == uuid)
                .ToListAsync();

            return result.Where(x => !x.IsDeleted).SingleOrDefault();
                
        }

        public async Task InsertAsync<T>(T record) where T : Entity
        {
            record.CreatedAt = DateTime.UtcNow;
            record.Update();
            _context.Set<T>().Add(record);
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> ListAsync<T>() where T : Entity
        {
            var result = await _context.Set<T>().ToListAsync();
            return result.Where(x => !x.IsDeleted).ToList();
        }

        public async Task RemoveAsync<T>(T record, bool isSoftDelete = true) where T : Entity
        {
            if (isSoftDelete)
            {
                record.DeletedAt = DateTime.UtcNow;
                record.Update();
            }
            else
            {
                _context.Set<T>().Remove(record);
            }
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync<T>(Guid uuid, bool isSoftDelete = true) where T : Entity
        {
            var record = await GetAsync<T>(uuid);
            if (record != null)
            {
                await RemoveAsync(record);
            }
        }

        public async Task UpdateAsync<T>(T record) where T : Entity
        {
            record.Update();
            _context.Set<T>().Update(record);
            await _context.SaveChangesAsync();
        }
    }
}
