using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.IServices;
using BLL.Models;
using DAL.UOW;

namespace BLL.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork unitOfWork;
        IMapper mapper;
        public BookService(IUnitOfWork uow, IMapper mapper)
        {
            this.mapper = mapper;
            unitOfWork = uow;
        }
        public async Task<Book> Add(Book book)
        {
            var b = mapper.Map<DAL.Entities.Book>(book);
            unitOfWork.BookRepository.Create(b);
            await unitOfWork.Save();
            return mapper.Map<Book>(b);
        }

        public async Task AddAuthor(int id, Author author)
        {
            await unitOfWork.BookRepository.AddAuthor(id, mapper.Map<DAL.Entities.Author>(author));
            await unitOfWork.Save();

        }
        public async Task AddAuthors(int id, ICollection<Author> authors)
        {
            await unitOfWork.BookRepository.AddAuthors(id, mapper.Map<ICollection<DAL.Entities.Author>>(authors));
            await unitOfWork.Save();
        }

        public async Task AddGenre(int id, Genre genre)
        {
            await unitOfWork.BookRepository.AddGenre(id, mapper.Map<DAL.Entities.Genre>(genre));
            await unitOfWork.Save();
        }

        public async Task AddGenres(int id, ICollection<Genre> genres)
        {
            await unitOfWork.BookRepository.AddGenres(id, mapper.Map<ICollection<DAL.Entities.Genre>>(genres));
            await unitOfWork.Save();
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            return mapper.Map<IEnumerable<Book>>(await unitOfWork.BookRepository.GetBooks());
        }

        public async Task<Book> GetById(int id)
        {
            return mapper.Map<Book>(await unitOfWork.BookRepository.GetBookById(id));
        }
    }
}
