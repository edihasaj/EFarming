using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EFarming.Models
{
    public class Farm
    {
        [Key]
        public int FarmId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string FullName => $"{FarmId}/{Name}/{Location}";
        public List<FarmZone> Zones { get; set; }
    }
}