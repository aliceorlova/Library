using BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> Add(User user);
        Task<User> GetById(int id);
        Task Update(User userParam, string password = null);
        Task<User> AssignRole(int id);
        Task Delete(int id);
        Task<IEnumerable<Booking>> GetBookings(int id);
        Task CreateRoles();
        Task<User> Login(User user);
    }
}
