using System.Collections.Generic;

namespace EFarming.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();  
        T Get(int id);  
        void Insert(T entity);  
        void Update(T entity);  
        void Delete(int id);
    }
}