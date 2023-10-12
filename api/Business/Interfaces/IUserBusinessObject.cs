using Business.Base;
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

    }
}
