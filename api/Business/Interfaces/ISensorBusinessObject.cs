using Business.Base;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ISensorBusinessObject
    {
        public Task<OperationResult> CreateSensor(Sensor record, string token);
        public Task<OperationResult> EditSensor(Guid uuid, Sensor record, string token);


    }
}
