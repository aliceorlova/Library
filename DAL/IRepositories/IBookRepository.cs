using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.IRepositories
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<IEnumerable<Book>> GetBooksAsync();
        Task<Book> GetBookByIdAsync(int id);
        Task AddAuthorAsync(int id, Author author);
        Task AddGenreAsync(int id, Genre genre);
    }
}
