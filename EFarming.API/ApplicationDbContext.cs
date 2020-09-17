using Microsoft.EntityFrameworkCore;
using EFarming.Models;

namespace EFarming.API
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<EFarming.Models.State> State { get; set; }
        public DbSet<EFarming.Models.Sensor> Sensor { get; set; }
        public DbSet<EFarming.Models.Actuator> Actuator { get; set; }
        public DbSet<EFarming.Models.FarmZone> FarmZone { get; set; }
        public DbSet<EFarming.Models.Farm> Farm { get; set; }
        public DbSet<EFarming.Models.PredefinedValues> PredefinedValues { get; set; }
        public DbSet<EFarming.Models.IrrigationMode> IrrigationMode { get; set; }
        public DbSet<EFarming.Models.SensorData> SensorData { get; set; }
    }
}