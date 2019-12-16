using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepositories
{
    public interface IUserRepository : IRepository<AppUser>
    {
        public Task<IEnumerable<AppUser>> GetUsers();
       // public Task<IdentityResult> CreateAsync(AppUser user);

    }
}
