using BLL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.IServices
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book> GetByIdAsync(int id);
        Task<Book> AddAsync(Book book);
        Task AddAuthorAsync(int id, Author author);
        Task AddGenreAsync(int id, Genre genre);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, Book book);
    }
}
