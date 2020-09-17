namespace EFarming.Models
{
    public class SimTracker
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string SimProvider { get; set; }
    }
}