using EFarming.Models;

namespace EFarming.Repository
{
    public interface IActuator
    {
        void CreateActuator(Actuator actuator);
        void RemoveActuator(int id);
        void OpenValve(int id);
        void CloseValve(int id);
    }
}