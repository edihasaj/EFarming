using System.Collections.Generic;
using EFarming.Models;

namespace EFarming.Repository
{
    public interface IFarmRepository<T> where T : class
    {
        void AddFarmZone(int farmId, FarmZone zone);
        void UpdateFarmZone(int farmId, FarmZone zone);
        void RemoveFarmZone(int farmId, int zoneId);
    }
}