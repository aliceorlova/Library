using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<IEnumerable<User>> GetUsers();
    }
}
