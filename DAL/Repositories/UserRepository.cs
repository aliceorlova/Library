using DAL.Entities;
using DAL.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    class UserRepository : Repository<AppUser>, IUserRepository
    {
        public UserRepository(AppContext context) : base(context) { }
        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await context.Set<AppUser>().Include(a => a.Bookings).ToListAsync();
        }
        public async Task<IEnumerable<AppUser>> GetCustomers()
        {
            return await context.Set<AppUser>().Include(a => a.Bookings).ToListAsync();
        }
    }
}
