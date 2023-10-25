using Business.Base;
using Business.BusinessModels;
using Business.Interfaces;
using Data.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.BusinessObjects
{
    public class ApiBusinessObject: AbstractBusinessObject, IApiBusinessObject
    {
        public async Task<OperationResult<VersionInfoBusinessModel>> GetApiVersion()
        {
            return await ExecuteOperation(async () =>
            {

                IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

                var versionInfo = new VersionInfoBusinessModel {
                    Version = configuration["ApiVersion:Version"],
                    VersionDate = DateTime.UtcNow.ToString("o")
                };

                return versionInfo;
            });
        }
    }
}
