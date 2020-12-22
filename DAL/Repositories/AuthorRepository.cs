using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using DAL.IRepositories;

namespace DAL.Repositories
{
    class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(AppContext context) : base(context) { }

        public async Task<Author> GetAuthorByIdAsync(int id)
        {
            return await context.Set<Author>().Where(a => a.AuthorId == id).Include(a => a.BookAuthors).ThenInclude(a => a.Book).SingleOrDefaultAsync();
        }

        public async Task<Author> GetByName(string firstName, string lastName)
        {
            return await context.Set<Author>().Where(a => a.FirstName == firstName && a.LastName == lastName).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            return await context.Set<Author>().Include(a => a.BookAuthors).ThenInclude(a => a.Book).ToListAsync();
        }
    }
}
