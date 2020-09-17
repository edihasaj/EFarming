using System.Collections.Generic;
using System.Linq;
using EFarming.Models;

namespace EFarming.Repository
{
    public class ActuatorRepository : IRepository<Actuator>
    {
        readonly List<Actuator> actuators;

        public ActuatorRepository()
        {
            actuators = new List<Actuator>();

            var sensorRepository = new SensorRepository();
            var sensorOne = sensorRepository.Get(1);
            var sensorTwo = sensorRepository.Get(2);

            actuators.Add(
                new Actuator
                {
                    Id = 1,
                    Name = "Water Valve",
                    Type = "Water Valve 3/4",
                    Latitude = 42.45555,
                    Longitude = 26.55524,
                    CriticalValue = 95,
                    IsOpen = false,
                    IsGoodCondition = true,
                    Sensors = new List<Sensor>
                    {
                        sensorOne,
                        sensorTwo
                    }
                }
            );
        }

        public IEnumerable<Actuator> GetAll()
        {
            return actuators;
        }

        public Actuator Get(int id)
        {
            return actuators.FirstOrDefault(a => a.Id == id);
        }

        public void Insert(Actuator entity)
        {
            if (!actuators.Any())
            {
                entity.Id = 1;
            }
            else
            {
                int lastId = actuators.Last().Id;
                entity.Id = ++lastId;
            }

            actuators.Add(entity);
        }

        public void Update(Actuator entity)
        {
            if (entity.Id < 1)
                return;

            actuators.Remove(actuators.FirstOrDefault(a => a.Id == entity.Id));
            actuators.Add(entity);
        }

        public void Delete(int id)
        {
            actuators.Remove(actuators.FirstOrDefault(a => a.Id == id));
        }

        public void SetValveCondition(int id, bool isGoodCondition)
        {
            actuators.FirstOrDefault(a => a.Id == id).IsGoodCondition = isGoodCondition;
        }

        public void OpenValve(int id)
        {
            actuators.FirstOrDefault(a => a.Id == id).IsOpen = true;
        }

        public void CloseValve(int id)
        {
            actuators.FirstOrDefault(a => a.Id == id).IsOpen = false;
        }

        public void OpenValveWithFlowRate(int id, double flowRate)
        {
            var actuator = actuators.FirstOrDefault(a => a.Id == id);
            actuator.IsOpen = true;
            actuator.WaterFlowRate = flowRate;
        }

        public void DecreaseValveFlowRate(int id, double flowRate)
        {
            var actuator = actuators.FirstOrDefault(a => a.Id == id);

            if (actuator.WaterFlowRate <= 5)
                actuator.IsOpen = false;
            else if (flowRate <= 5)
                actuator.IsOpen = false;
            else
                actuator.IsOpen = true;

            actuator.WaterFlowRate = flowRate;
        }
    }
}