using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.IRepositories;

namespace DAL.Repositories
{
    class GenreRepository : Repository<Genre>, IGenreRepository
    {
        public GenreRepository(AppContext context) : base(context) { }

        public async Task<Genre> GetGenreByIdAsync(int id)
        {
            return await context.Set<Genre>().Where(a => a.GenreId == id).Include(a => a.BookGenres).ThenInclude(a => a.Book).SingleOrDefaultAsync();
        }
        public async Task<IEnumerable<Genre>> GetGenresAsync()
        {
            return await context.Set<Genre>().AsNoTracking().Include(a => a.BookGenres).ThenInclude(a => a.Book).ToListAsync();
        }
        public async Task<Genre> GetByName(string name)
        {
            return await context.Set<Genre>().Where(g => g.Name == name).SingleOrDefaultAsync();
        }
    }
}
