using System;
using System.Collections.Generic;

namespace EFarming.Models
{
    public class Sensor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SensorType Type { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double CriticalValue { get; set; }

        public List<SensorData> Data { get; set; }
        
    }

    public class SensorData
    {
        public int Id { get; set; }
        public double Signal { get; set; }
        public DateTime CreatedAt { get; set; }
        public int SensorId { get; set; }
        public Sensor Sensor { get; set; }
    }
}
