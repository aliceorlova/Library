using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
        // need it to get the valid id
        public async Task<Genre> Add(Genre genre)
        {
            var a = mapper.Map<DAL.Entities.Genre>(genre);
            unitOfWork.GenreRepository.Create(a);
            await unitOfWork.Save();
            return mapper.Map<Genre>(a);
        }

        public async Task<IEnumerable<Genre>> GetAll()
        {
            return mapper.Map<List<Genre>>(await unitOfWork.GenreRepository.GetAll());
        }

        public async Task<Genre> GetById(int id)
        {
            return mapper.Map<Genre>(await unitOfWork.GenreRepository.GetById(id));
        }
    }
}
