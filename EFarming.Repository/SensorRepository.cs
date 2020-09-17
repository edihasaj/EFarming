using EFarming.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFarming.Repository
{
    public class SensorRepository : IRepository<Sensor>
    {
        readonly List<Sensor> sensors = new List<Sensor>()
        {
            new Sensor
            {
                Name = "Soil Moisture Sensor",
                Type = SensorType.Moisture,
                Latitude = 42.45565,
                Longitude = 26.55564,
                // CreatedAt = DateTime.Now,
                // Signal = 0
            },
            new Sensor
            {
                Name = "Temperature Sensor",
                Type = SensorType.Temperature,
                Latitude = 42.45577,
                Longitude = 26.55579,
                CriticalValue = 80,
                // CreatedAt = DateTime.Now,
                // Signal = 0
            },
            new Sensor
            {
                Name = "Humidity",
                Type = SensorType.Humidity,
                Latitude = 42.45572,
                Longitude = 26.55539,
                CriticalValue = 70,
                // CreatedAt = DateTime.Now,
                // Signal = 0
            },
        };

        public void Delete(int id)
        {
            sensors.Remove(sensors.FirstOrDefault(s => s.Id == id));
        }

        public Sensor Get(int id)
        {
            return sensors.FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<Sensor> GetAll()
        {
            return sensors;
        }

        public void Insert(Sensor entity)
        {
            if (!sensors.Any())
            {
                entity.Id = 1;
            }
            else
            {
                int lastId = sensors.Last().Id;
                entity.Id = ++lastId;
            }

            sensors.Add(entity);
        }

        public void Update(Sensor entity)
        {
            if (entity.Id < 1)
                return;

            sensors.Remove(sensors.FirstOrDefault(s => s.Id == entity.Id));
            sensors.Add(entity);
        }
    }
}
