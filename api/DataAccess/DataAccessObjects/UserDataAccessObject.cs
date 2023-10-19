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
    public class UserDataAccessObject : IUserDataAccessObject
    {
        protected readonly UnoOnBoardingContext _context;

        public UserDataAccessObject(UnoOnBoardingContext context)
        {
            _context = context;
        }

        public async Task<List<User>> FilterUsers(string search,int sort)
        {
            if (sort == 0)
            {
                List<User> result = await _context.Set<User>()
                .Where(x => (x.Name!.ToLower().Contains(search.ToLower())) ||
                            (x.Email!.ToLower().Contains(search.ToLower())))
                .OrderBy(x => x.Name).ToListAsync();

                return result.Where(x => !x.IsDeleted).ToList(); ;
            }

            else
            {
                List<User> result = await _context.Set<User>()
                .Where(x => (x.Name!.ToLower().Contains(search.ToLower())) ||
                            (x.Email!.ToLower().Contains(search.ToLower())))
                .OrderByDescending(x => x.Name).ToListAsync();

            return result.Where(x => !x.IsDeleted).ToList();
            }
        }


    }
}
