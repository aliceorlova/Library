using DAL.Entities;
using System.Threading.Tasks;
using DAL.IRepositories;

namespace DAL.UOW
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IGenreRepository GenreRepository { get; }
        IBookRepository BookRepository { get; }
        IRepository<BookAuthor> BookAuthorRepository { get; }
        IRepository<BookGenre> BookGenreRepository { get; }
        IBookingRepository BookingRepository { get; }
        IAuthorRepository AuthorRepository { get; }
        Task SaveAsync();
    }
}
