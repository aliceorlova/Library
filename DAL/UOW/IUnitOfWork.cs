using DAL.Entities;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.IRepositories;

namespace DAL.UOW
{
    public interface IUnitOfWork
    {
        IRepository<User> UserRepository { get; }
        IGenreRepository GenreRepository { get; }
        IBookRepository BookRepository { get; }
        IRepository<BookAuthor> BookAuthorRepository { get; }
        IRepository<BookGenre> BookGenreRepository { get; }
        IBookingRepository BookingRepository { get; }
        IAuthorRepository AuthorRepository { get; }
        Task Save();
    }
}
