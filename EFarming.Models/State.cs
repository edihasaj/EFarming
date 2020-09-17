using System;

namespace EFarming.Models
{
    public class State
    {
        public int Id { get; set; }
        public int ActuatorId { get; set; }
        public DateTime OpenDate { get; set; }
        public bool IsOpen { get; set; }
    }
}