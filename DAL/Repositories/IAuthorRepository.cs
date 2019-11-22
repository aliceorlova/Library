using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IAuthorRepository:IRepository<Author>
    {
        public Task<IEnumerable<Author>> GetAuthors();
        public Task<Author> GetAuthorById(int id);
    }
}
