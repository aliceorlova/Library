using DAL.Entities;
using DAL.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppContext context) : base(context) { }
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await context.Users.Include(a => a.Bookings).ToListAsync();
        }
    }
}
