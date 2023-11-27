using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.BusinessModels
{
    public class SensorBusinessModel
    {
        public Guid Uuid { get; set; }
        public long UserId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsPublic { get; set; }
        public string? Category { get; set; }
        public string? Color { get; set; }

        public SensorBusinessModel(Sensor sensor) {
            Uuid = sensor.Uuid;
            UserId = sensor.UserId;
            Name = sensor.Name;
            Description = sensor.Description;
            IsPublic = sensor.IsPublic;
            Category = sensor.Category;
            Color = sensor.Color;
        }
    }
}
