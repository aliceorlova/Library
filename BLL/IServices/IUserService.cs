using BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IServices
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(int id);
        Task<User> Create(User user, string password);
        Task Update(User userParam, string password = null);
        Task<User> AssignRole(User user);
        Task Delete(int id);
        Task<IEnumerable<Booking>> GetBookings(int id);
    }
}
