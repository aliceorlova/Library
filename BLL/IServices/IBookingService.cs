using BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IServices
{
    public interface IBookingService
    {
        public Task<IEnumerable<Booking>> GetAll();
        public Task<Booking> GetById(int id);
        Task<Booking> Add(Booking booking);
        Task Delete(int id);
        Task Update(int id, Booking booking);
        Task<Booking> FinishBooking(Booking booking);
    }
}
