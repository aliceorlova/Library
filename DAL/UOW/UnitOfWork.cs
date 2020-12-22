using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.IRepositories;

[assembly: InternalsVisibleTo("Tests")]
namespace DAL.UOW
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly AppContext context;

        public IUserRepository UserRepository { get; }

        public IAuthorRepository AuthorRepository { get; set; }

        public IGenreRepository GenreRepository { get; set; }

        public IBookRepository BookRepository { get; set; }

        public IRepository<BookAuthor> BookAuthorRepository { get; }

        public IRepository<BookGenre> BookGenreRepository { get; }

        public IBookingRepository BookingRepository { get; set; }

        public UnitOfWork() { }

        public UnitOfWork(AppContext ac, IAuthorRepository authorRepository, IGenreRepository genreRepository,
           IBookRepository bookRepository, IRepository<BookAuthor> bookAuthorRepository, IRepository<BookGenre> bookGenreRepository,
           IUserRepository userRepository, IBookingRepository bookingRepository) 
        {
            context = ac;
            AuthorRepository = authorRepository;
            GenreRepository = genreRepository;
            BookRepository = bookRepository;
            BookAuthorRepository = bookAuthorRepository;
            BookGenreRepository = bookGenreRepository;
            UserRepository = userRepository;
            BookingRepository = bookingRepository;
        }
        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
