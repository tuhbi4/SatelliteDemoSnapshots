using System.Collections.Generic;
using System.Threading.Tasks;

namespace SatelliteDemoSnapshots.DemoSnapshots.DL.DAO.Interfaces
{
    public interface IRepository<T>
    {
        public Task<IReadOnlyList<T>> GetAllAsync(string query);

        public Task<int> CreateAsync(T entity);

        public Task<T> ReadAsync(int id);

        public Task<int> UpdateAsync(T entity);

        public Task<int> DeleteAsync(int id);
    }
}