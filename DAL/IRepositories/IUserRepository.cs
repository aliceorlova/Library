using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.IRepositories
{
    public interface IUserRepository : IRepository<AppUser>
    {
        Task<IEnumerable<AppUser>> GetUsersAsync();
    }
}
