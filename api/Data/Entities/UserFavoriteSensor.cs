using Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class UserFavoriteSensor : Entity
    {
        [ForeignKey("UserId")]
        public long? UserId { get; set; }
        public virtual User? User { get; set; }

        [ForeignKey("SensorId")]
        public long? SensorId { get; set; }
        public virtual Sensor? Sensor { get; set; }  
    }
}
