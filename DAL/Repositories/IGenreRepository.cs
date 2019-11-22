using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IGenreRepository : IRepository<Genre>
    {
        public Task<IEnumerable<Genre>> GetGenres();
        public Task<Genre> GetGenreById(int id);
    }
}
