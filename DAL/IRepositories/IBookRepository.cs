using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepositories
{
    public interface IBookRepository : IRepository<Book>
    {
        public Task<IEnumerable<Book>> GetBooks();
        public Task<Book> GetBookById(int id);
        public Task AddAuthor(int id, Author author);
      //  public Task AddAuthors(int id, ICollection<Author> authors);
        public Task AddGenre(int id, Genre genre);
      //  public Task AddGenres(int id, ICollection<Genre> genres);
    }
}
