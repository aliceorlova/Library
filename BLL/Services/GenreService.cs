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
    class GenreService : IGenreService
    {
        private readonly IUnitOfWork unitOfWork;
        IMapper mapper;
        public GenreService(IUnitOfWork uow, IMapper mapper)
        {
            this.mapper = mapper;
            unitOfWork = uow;
        }
        
        public async Task<Genre> Add(Genre genre)
        {
            var a = mapper.Map<DAL.Entities.Genre>(genre);
            unitOfWork.GenreRepository.Create(a);
            await unitOfWork.Save();
            return mapper.Map<Genre>(a);
        }

        public async Task Delete(int id)
        {
            unitOfWork.GenreRepository.Delete(await unitOfWork.GenreRepository.GetById(id));
            await unitOfWork.Save();
        }

        public async Task<IEnumerable<Genre>> GetAll()
        {
            return mapper.Map<IEnumerable<Genre>>(await unitOfWork.GenreRepository.GetGenres());
        }

        public async Task<Genre> GetById(int id)
        {
            return mapper.Map<Genre>(await unitOfWork.GenreRepository.GetGenreById(id));
        }

        public async Task Update(int id, Genre genre)
        {
            var existing = await unitOfWork.GenreRepository.GetById(id);
            //  if (existing== null) return error
            existing.Name = genre.Name;
            unitOfWork.GenreRepository.Update(existing);
            await unitOfWork.Save();
        }
    }
}
