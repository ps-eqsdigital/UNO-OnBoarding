using Business.Base;
using Business.BusinessModels;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IUserBusinessObject
    {
        public Task<OperationResult> Update(Guid uuid, User record);
        public Task<OperationResult<CreateUserBusinessModel>> Insert(User record);
        public Task<OperationResult<List<UserBusinessModel>>> ListFilteredUsers(string search, int sort);
        public Task<OperationResult<LoginBusinessModel>> Login(string email, string password);
        public Task<OperationResult<List<UserBusinessModel>>> GetUsers();
        public Task<OperationResult> Logout(string token);
        public Task<OperationResult> RecoverPassword(string email);
        public Task<OperationResult> ResetPassword(string passwordResetToken, string newPassword);



    }
}
