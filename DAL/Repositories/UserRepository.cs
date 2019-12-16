using DAL.Entities;
using DAL.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    class UserRepository : Repository<AppUser>, IUserRepository
    {
        public UserRepository(AppContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AppUser>> GetUsers()
        {
            return await context.Set<AppUser>().Include(a => a.Bookings).ToListAsync();
        }

    }
}
