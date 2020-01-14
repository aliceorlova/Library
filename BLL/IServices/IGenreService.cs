using BLL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.IServices
{
    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetAllAsync();
        Task<Genre> GetByIdAsync(int id);
        Task<Genre> AddAsync(Genre genre);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, Genre genre);
    }
}
