using System;
using System.Collections.Generic;

namespace EFarming.Models
{
    public class Actuator
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double CriticalValue { get; set; }
        public bool IsOpen { get; set; }
        public DateTime ValveOpenTime { get; set; }
        public double WaterFlowRate { get; set; }
        public bool IsGoodCondition { get; set; }

        public int FarmZoneId { get; set; }
        public virtual FarmZone FarmZone { get; set; }
        
        public List<Sensor> Sensors { get; set; }

        public ICollection<ProcessedData> ProcessedData { get; set; }
    }
}