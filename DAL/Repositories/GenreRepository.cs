using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IRepositories;

namespace DAL.Repositories
{
    class GenreRepository : Repository<Genre>, IGenreRepository
    {
        public GenreRepository(AppContext context) : base(context) { }

        public async Task<Genre> GetGenreById(int id)
        {
            return await context.Genres.Where(a => a.GenreId == id).Include(a => a.BookGenres).ThenInclude(a => a.Genre).FirstAsync();
        }

        public async Task<IEnumerable<Genre>> GetGenres()
        {
            return await context.Genres.AsNoTracking().Include(a => a.BookGenres).ThenInclude(a => a.Genre).ToListAsync();
        }
    }
}
