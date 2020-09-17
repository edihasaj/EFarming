using System.Collections.Generic;

namespace EFarming.Models
{
    public class ProcessedData
    {
        public int Id { get; set; }
        
        public int ActuatorId { get; set; }
        public Actuator Actuator { get; set; }
        
        public State State { get; set; }
        public IrrigationMode IrrigationMode { get; set; }
    }
}
