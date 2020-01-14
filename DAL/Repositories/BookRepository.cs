using DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DAL.IRepositories;

namespace DAL.Repositories
{
    class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(AppContext context) : base(context) { }

        public async Task AddAuthorAsync(int id, Author author)
        {
            var book = await GetByIdAsync(id);
            Author a = await context.Set<Author>().FindAsync(author.AuthorId);
            context.Set<BookAuthor>().Add(new BookAuthor { Book = book, Author = a });
        }
        public async Task AddGenreAsync(int id, Genre genre)
        {
            var book = await GetByIdAsync(id);
            Genre g = await context.Set<Genre>().FindAsync(genre.GenreId);
            context.Set<BookGenre>().Add(new BookGenre { Book = book, Genre = g });
        }
        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await context.Set<Book>().Where(b => b.BookId == id).Include(b => b.BookAuthors).ThenInclude(a => a.Author).Include(b => b.BookGenres).ThenInclude(g => g.Genre).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await context.Set<Book>().Include(a => a.BookAuthors).ThenInclude(a => a.Author).Include(a => a.BookGenres).ThenInclude(a => a.Genre).ToListAsync();
        }
    }
}
