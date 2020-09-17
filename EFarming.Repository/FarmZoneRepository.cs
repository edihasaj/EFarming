using EFarming.Models;
using System.Collections.Generic;
using System.Linq;

namespace EFarming.Repository
{
    public class FarmZoneRepository : IRepository<FarmZone>
    {
        readonly List<FarmZone> farmZones;

        public FarmZoneRepository()
        {
            farmZones = new List<FarmZone>();

            var actuatorRepository = new ActuatorRepository();
            var actuator = actuatorRepository.Get(1);

            farmZones.Add(
                new FarmZone
                {
                    ZoneId = 1,
                    Name = "Zona 1",
                    Code = "Z1",
                    Latitude = 42.45555,
                    Longitude = 26.55524,
                    FarmId = 1,
                    Actuators = new List<Actuator>
                    {
                        actuator
                    }
                }
            );
        }

        public void Delete(int id)
        {
            farmZones.Remove(farmZones.FirstOrDefault(f => f.ZoneId == id));
        }

        public FarmZone Get(int id)
        {
            return farmZones.FirstOrDefault(f => f.ZoneId == id);
        }

        public IEnumerable<FarmZone> GetAll()
        {
            return farmZones;
        }

        public void Insert(FarmZone entity)
        {
            if (!farmZones.Any())
            {
                entity.ZoneId = 1;
            }
            else
            {
                int lastId = farmZones.Last().ZoneId;
                entity.ZoneId = ++lastId;
            }

            farmZones.Add(entity);
        }

        public void Update(FarmZone entity)
        {
            if (entity.ZoneId < 1)
                return;

            farmZones.Remove(farmZones.FirstOrDefault(a => a.ZoneId == entity.ZoneId));
            farmZones.Add(entity);
        }
    }
}
