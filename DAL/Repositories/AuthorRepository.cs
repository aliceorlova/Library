using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(AppContext context) : base(context) { }

        public async Task<Author> GetAuthorById(int id)
        {
            return await context.Authors.Where(a => a.AuthorId == id).Include(a => a.BookAuthors).ThenInclude(a => a.Author).FirstAsync();
        }

        public async Task<IEnumerable<Author>> GetAuthors()
        {
            return await context.Authors.Include(a => a.BookAuthors).ThenInclude(a => a.Book).ToListAsync();
        }
    }
}
