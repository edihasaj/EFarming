namespace EFarming.Models
{
    public class PredefinedValues
    {
        public int Id { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public double Value { get; set; }
        public string ValueType { get; set; }
        public SensorType ValueFor { get; set; }
    }
}
