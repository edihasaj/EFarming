using EFarming.Models;
using System.Collections.Generic;
using System.Linq;

namespace EFarming.Repository
{
    public class PredefinedValuesRepo : IRepository<PredefinedValues>
    {
        static List<PredefinedValues> predefinedValues = new List<PredefinedValues>() 
        {
           /* new PredefinedValues
            {
                Id = 1,
                MinValue = 20,
                MaxValue = 100,
                Value = 60,
                ValueType = "%",
                ValueFor = "Humidity"
            },
            new PredefinedValues
            {
                Id = 2,
                MinValue = -40,
                MaxValue = 80,
                Value = 22,
                ValueType = "Celcius",
                ValueFor = "Temperature"
            },
            new PredefinedValues
            {
                Id = 3,
                MinValue = 50,
                MaxValue = 1023,
                Value = 200,
                ValueType = "bit",
                ValueFor = "Moisture"
            }*/
        };

        public PredefinedValues Get(int id)
        {
            return predefinedValues.FirstOrDefault(pv => pv.Id == id);
        }

        public IEnumerable<PredefinedValues> GetAll()
        {
            return predefinedValues;
        }

        public void Insert(PredefinedValues entity)
        {
            if (!predefinedValues.Any())
            {
                entity.Id = 1;
            } else
            {
                int lastId = predefinedValues.Last().Id;
                entity.Id = ++lastId;
            }

            predefinedValues.Add(entity);
        }

        public void Update(PredefinedValues entity)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int id)
        {
            predefinedValues.Remove(predefinedValues.Single(x => x.Id == id));
        }
    }
}
