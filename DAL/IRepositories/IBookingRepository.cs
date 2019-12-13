using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepositories
{
    public interface IBookingRepository : IRepository<Booking>
    {
        public Task<IEnumerable<Booking>> GetBookings();
        public Task<Booking> GetBookingById(int id);
        public Task<IEnumerable<Booking>> GetActiveBookings();
    }
}
