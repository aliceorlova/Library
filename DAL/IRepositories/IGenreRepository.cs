using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.IRepositories
{
    public interface IGenreRepository : IRepository<Genre>
    {
        Task<IEnumerable<Genre>> GetGenresAsync();
        Task<Genre> GetGenreByIdAsync(int id);
        Task<Genre> GetByName(string name);
    }
}
