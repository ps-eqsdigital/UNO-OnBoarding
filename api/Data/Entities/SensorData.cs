using Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class SensorData:Entity
    {
        [ForeignKey("Sensor")]
        public long? SensorId { get; set; }
        public virtual Sensor? Sensor { get; set; }
        public DateTime? TimeStamp { get; set; }
        public double? Value { get; set; }
    }
}
