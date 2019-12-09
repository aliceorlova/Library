using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.IRepositories;

namespace DAL.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppContext context;
        public IUserRepository UserRepository { get; }
        public IAuthorRepository AuthorRepository { get; }

        public IGenreRepository GenreRepository { get; }

        public IBookRepository BookRepository { get; }

        public IRepository<BookAuthor> BookAuthorRepository { get; }

        public IRepository<BookGenre> BookGenreRepository { get; }
        public IBookingRepository BookingRepository { get; }

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
        public async Task Save()
        {
            await context.SaveChangesAsync();
        }
    }
}
