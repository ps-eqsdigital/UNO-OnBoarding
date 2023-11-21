using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace api.Requests
{
    public class SensorDataRequest
    {
        public Guid SensorUuid { get; set; }
        public List<List<JsonElement>>? SensorData { get; set; }

        public List<SensorData> ToSensorData()
        {
            List<SensorData> sensorDataList = new List<SensorData>();

            if (SensorData != null)
            {
                foreach (var dataPoint in SensorData)
                {
                    if (dataPoint.Count >= 2 && dataPoint[0].ValueKind == JsonValueKind.String && dataPoint[1].ValueKind == JsonValueKind.Number)
                    {
                        var timestampString = dataPoint[0].GetString();
                        var value = dataPoint[1].GetDouble();

                        if (DateTime.TryParse(timestampString, null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime timestamp))
                        {
                            var sensorData = new SensorData()
                            {
                                Uuid = Guid.NewGuid(),
                                TimeStamp = timestamp,
                                Value = value
                            };
                            sensorDataList.Add(sensorData);
                        }
                    }
                }
            }
            return sensorDataList;
        }
    }
}
