using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Repositories;

namespace DAL.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppContext context;
        public IRepository<Author> AuthorRepository { get; }
        
        public IRepository<Genre> GenreRepository { get; }

        public IBookRepository BookRepository { get; }

        public IRepository<BookAuthor> BookAuthorRepository { get; }

        public IRepository<BookGenre> BookGenreRepository { get; }

        public UnitOfWork(AppContext ac, IRepository<Author> authorRepository, IRepository<Genre> genreRepository,
           IBookRepository bookRepository, IRepository<BookAuthor> bookAuthorRepository, IRepository<BookGenre> bookGenreRepository)
        {
            context = ac;
            AuthorRepository = authorRepository;
            GenreRepository = genreRepository;
            BookRepository = bookRepository;
            BookAuthorRepository = bookAuthorRepository;
            BookGenreRepository = bookGenreRepository;
        }



        public async Task Save()
        {
            await context.SaveChangesAsync();
        }
    }
}
