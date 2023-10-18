using Business.Base;
using Business.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IApiBusinessObject
    {
        public Task<OperationResult<VersionInfoBusinessModel>> GetApiVersion();

    }
}
