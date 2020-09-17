namespace EFarming.Models.Interfaces
{
    public interface ISensor
    {
        public Sensor CreateSensor(Sensor sensor);
        public void RemoveSensor(int id);
        public double ReadInput(int id);
    }
}