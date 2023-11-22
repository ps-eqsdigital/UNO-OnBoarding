namespace api.Requests
{
    public class MarkFavSensorRequest
    {
        public Guid SensorUuid { get; set; }
        public bool Favorite { get; set; }
    }
}
