namespace EFarming.Models
{
    public class IrrigationMode
    {
        public int Id { get; set; }
        public IrrigationModeEnum Mode { get; set; } 
        public int FarmZoneId { get; set; }
        public FarmZone FarmZone { get; set; }
    }
}