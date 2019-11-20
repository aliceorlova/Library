using AutoMapper;
using BLL.Models;
using DAL.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork unitOfWork;
        IMapper mapper;
        public AuthorService(IUnitOfWork uow, IMapper mapper)
        {
            this.mapper = mapper;
            unitOfWork = uow;
        }

        public async Task<Author> Add(Author author)
        {
            var a = mapper.Map<DAL.Entities.Author>(author);
            unitOfWork.AuthorRepository.Create(a);
            await unitOfWork.Save();
            return mapper.Map<Author>(a);
        }

        public async Task<IEnumerable<Author>> GetAll()
        {
            return mapper.Map<List<Author>>(await unitOfWork.AuthorRepository.GetAll());

        }
        public async Task<Author> GetById(int id)
        {
            return mapper.Map<Author>(await unitOfWork.AuthorRepository.GetById(id));
        }
        public async Task Delete(int id)
        {
            unitOfWork.AuthorRepository.Delete(await unitOfWork.AuthorRepository.GetById(id));
            await unitOfWork.Save();
        }

        public async Task Update(int id, Author author)
        {

            var existing = await unitOfWork.AuthorRepository.GetById(id);
            //  if (existing== null) return error
            existing.FirstName = author.FirstName;
            existing.LastName = author.LastName;
            unitOfWork.AuthorRepository.Update(existing);
            await unitOfWork.Save();
        }
    }
}
