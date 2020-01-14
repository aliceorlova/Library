using BLL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> AddAsync(User user);
        Task<User> GetByIdAsync(int id);
        Task UpdateAsync(User userParam, string password = null);
        Task<User> AssignRoleAsync(int id);
        Task DeleteAsync(int id);
        Task<IEnumerable<Booking>> GetBookingsAsync(int id);
        Task<IEnumerable<User>> GetCustomersAsync();
        Task<IEnumerable<User>> GetCustomersAndManagersAsync();
        Task<User> LoginAsync(User user);
    }
}
