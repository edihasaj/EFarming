using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFarming.Models
{
    public class FarmZone
    {
        [Key]
        public int ZoneId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int FarmId { get; set; }
        public string FullName => $"{Name}/{Code}";
        public virtual Farm Farm { get; set; }
        
        public List<Actuator> Actuators { get; set; }
    }
}