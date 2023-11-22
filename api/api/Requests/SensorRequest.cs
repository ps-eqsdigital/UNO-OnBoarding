using Data.Entities;

namespace api.Requests
{
    public class SensorRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsPublic { get; set; }
        public string? Category { get; set; }
        public string? Color { get; set; }

        public Sensor ToSensor()
        {
            return new Sensor {Name = Name, Description = Description, IsPublic = IsPublic, Category = Category, Uuid = Guid.NewGuid(), Color=Color };
        }
    }
}
