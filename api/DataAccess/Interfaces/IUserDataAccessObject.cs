using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IUserDataAccessObject
    {
        public Task<List<User>> FilterUsers(string search,int sort);
        public Task<User> GetUserByEmail(string email);
        public Task<UserTokenAuthentication> GetUserTokenByUserId(long id);
        public Task<UserTokenAuthentication> GetToken(string token);

    }
}
