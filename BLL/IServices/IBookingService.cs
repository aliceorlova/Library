using BLL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.IServices
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllAsync();
        Task<Booking> GetByIdAsync(int id);
        Task<Booking> AddAsync(Booking booking);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, Booking booking);
        Task<Booking> FinishBookingAsync(int id);
        Task<IEnumerable<Booking>> GetAllActiveAsync();
    }
}
