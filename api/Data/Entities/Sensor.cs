using Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Sensor:Entity
    {
        [ForeignKey("UserId")]
        public long UserId { get; set; }
        public virtual User? User { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsPublic { get; set; }
        public string? Category { get; set; }
        public string? Color { get; set; }
        public ICollection<SensorData>? SensorDatas { get; set; }
        public virtual ICollection<UserFavoriteSensor>? FavoriteSensors { get; set; }

    }
}
