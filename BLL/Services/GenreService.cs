using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BLL.IServices;
using BLL.Models;
using DAL.UOW;

namespace BLL.Services
{
    public class GenreService : IGenreService
    {
        readonly IUnitOfWork _unitOfWork;
        IMapper _mapper;
        public GenreService(IUnitOfWork uow, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = uow;
        }

        public async Task<Genre> AddAsync(Genre genre)
        {
            var a = _mapper.Map<DAL.Entities.Genre>(genre);
            var existing = await _unitOfWork.GenreRepository.GetByName(genre.Name);
            if (existing != null) throw new AppException("Such genre already exists.");
            _unitOfWork.GenreRepository.Create(a);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<Genre>(a);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.GenreRepository.GetByIdAsync(id);
            if (existing == null) throw new AppException("Can`t delete the genre. Wrong id.");
            _unitOfWork.GenreRepository.Delete(existing);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<Genre>>(await _unitOfWork.GenreRepository.GetGenresAsync());
        }

        public async Task<Genre> GetByIdAsync(int id)
        {
            if (id < 0) throw new AppException("Index can`t be less than 0.");
            var existing = await _unitOfWork.GenreRepository.GetByIdAsync(id);
            if (existing == null) throw new AppException("Genre with such id does not exist");
            return _mapper.Map<Genre>(existing);
        }

        public async Task UpdateAsync(int id, Genre genre)
        {
            var existing = await _unitOfWork.GenreRepository.GetByIdAsync(id);
            if (existing == null) throw new AppException("Can`t update the genre. Wrong id.");
            existing.Name = genre.Name;
            _unitOfWork.GenreRepository.Update(existing);
            await _unitOfWork.SaveAsync();
        }
    }
}
