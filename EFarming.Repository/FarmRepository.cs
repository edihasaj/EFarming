using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using EFarming.Models;

namespace EFarming.Repository
{
    public class FarmRepository : IRepository<Farm>
    {
        private readonly List<Farm> _farms = new List<Farm>()
        {
            new Farm()
            {
                FarmId = 1, Name = "Farma e pare", Location = "Dikun",
                Zones = new List<FarmZone>()
            },
            new Farm()
            {
                FarmId = 2, Name = "Farma e dyte", Location = "Dikun tjeter",
                Zones = new List<FarmZone>()
            }
        };
        
        public IEnumerable<Farm> GetAll()
        {
            return _farms;
        }

        public Farm Get(int id)
        {
            foreach (var item in _farms)
            {
                if (item.FarmId == id)
                    return item;
            }

            throw new ArgumentNullException();
        }

        public void Insert(Farm entity)
        {
            _farms.Add(entity);
        }

        public void Update(Farm entity)
        {
            _farms.Remove(_farms.Single(x=>x.FarmId == entity.FarmId));
            _farms.Add(entity);
        }

        public void Delete(int id)
        {
            _farms.Remove(_farms.Single(x=>x.FarmId == id));
        }
    }
}