using DAL.Entities;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UOW
{
    public interface IUnitOfWork
    {
        IRepository<User> UserRepository { get; }
        IRepository<Genre> GenreRepository { get; }
        IBookRepository BookRepository { get; }
        IRepository<BookAuthor> BookAuthorRepository { get; }
        IRepository<BookGenre> BookGenreRepository { get; }
        //IRepository<Booking> BookingRepository { get; }
        IAuthorRepository AuthorRepository { get; }
        Task Save();
    }
}
