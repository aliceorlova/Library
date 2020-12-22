using AutoMapper;
using BLL.IServices;
using BLL.Models;
using DAL.UOW;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AuthorService : IAuthorService
    {
        readonly IUnitOfWork _unitOfWork;
        IMapper _mapper;
        public AuthorService(IUnitOfWork uow, IMapper mapper)
        {
            this._mapper = mapper;
            _unitOfWork = uow;
        }

        public async Task<Author> AddAsync(Author author)
        {
            var a = _mapper.Map<DAL.Entities.Author>(author);
            var existing = await _unitOfWork.AuthorRepository.GetByName(a.FirstName, a.LastName);
            if (existing != null) throw new AppException("Author with such full name already exists.");
            _unitOfWork.AuthorRepository.Create(a);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<Author>(a);
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<Author>>(await _unitOfWork.AuthorRepository.GetAuthorsAsync());
        }

        public async Task<Author> GetByIdAsync(int id)
        {
            if (id < 0) throw new AppException("Index can`t be less than 0.");
            var existing = await _unitOfWork.AuthorRepository.GetAuthorByIdAsync(id);
            if (existing == null) throw new AppException("Author with such id does not exist");
            return _mapper.Map<Author>(existing);
        }
        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.AuthorRepository.GetByIdAsync(id);
            if (existing == null) throw new AppException("Author you are trying to delete does not exist.");
            _unitOfWork.AuthorRepository.Delete(existing);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(int id, Author author)
        {
            var existing = await _unitOfWork.AuthorRepository.GetByIdAsync(id);
            if (existing == null) throw new AppException("Author you are trying to update does not exist.");
            existing.FirstName = author.FirstName;
            existing.LastName = author.LastName;
            _unitOfWork.AuthorRepository.Update(existing);
            await _unitOfWork.SaveAsync();
        }
    }
}
