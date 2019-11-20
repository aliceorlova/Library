using BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
   public interface IBookService
    {
        public Task<IEnumerable<Book>> GetAll();
        public Task<Book> GetById(int id);
        public Task<Book> Add(Book book);
        public Task AddAuthors(int id, ICollection<Author> authors);
        public Task AddAuthor(int id, Author author);
        public Task AddGenres(int id, ICollection<Genre> genres);
        public Task AddGenre(int id, Genre genre);
    }
}
