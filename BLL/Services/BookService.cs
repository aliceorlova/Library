using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BLL.IServices;
using BLL.Models;
using DAL.UOW;

namespace BLL.Services
{
    public class BookService : IBookService
    {
        readonly IUnitOfWork _unitOfWork;
        IMapper _mapper;
        public BookService(IUnitOfWork uow, IMapper mapper)
        {
            this._mapper = mapper;
            _unitOfWork = uow;
        }
        public async Task<Book> AddAsync(Book book)
        {
            var b = _mapper.Map<DAL.Entities.Book>(book);
            _unitOfWork.BookRepository.Create(b);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<Book>(b);
        }

        public async Task AddAuthorAsync(int id, Author author)
        {
            await _unitOfWork.BookRepository.AddAuthorAsync(id, _mapper.Map<DAL.Entities.Author>(author));
            await _unitOfWork.SaveAsync();

        }

        public async Task AddGenreAsync(int id, Genre genre)
        {
            await _unitOfWork.BookRepository.AddGenreAsync(id, _mapper.Map<DAL.Entities.Genre>(genre));
            await _unitOfWork.SaveAsync();
        }


        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.BookRepository.GetByIdAsync(id);
            if (existing == null) throw new AppException("Can`t delete the book. Wrong id.");
            _unitOfWork.BookRepository.Delete(existing);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<Book>>(await _unitOfWork.BookRepository.GetBooksAsync());
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            var existing = await _unitOfWork.BookRepository.GetByIdAsync(id);
            if (existing == null) throw new AppException("Book with such id does not exist.");
            return _mapper.Map<Book>(await _unitOfWork.BookRepository.GetBookByIdAsync(id));
        }

        public async Task UpdateAsync(int id, Book book)
        {
            var existing = await _unitOfWork.BookRepository.GetByIdAsync(id);
            if (existing == null) throw new AppException("Can`t update the book. Wrong id.");
            existing.Name = book.Name;
            existing.BookYear = book.BookYear;
            existing.NumberAvailable = book.NumberAvailable;
           
            _unitOfWork.BookRepository.Update(existing);
            await _unitOfWork.SaveAsync();
        }
    }
}
