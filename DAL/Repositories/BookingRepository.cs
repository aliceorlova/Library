using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IRepositories;

namespace DAL.Repositories
{
   internal class BookingRepository: Repository<Booking>, IBookingRepository
    {
        public BookingRepository(AppContext context) : base(context) { }

        public async Task<IEnumerable<Booking>> GetActiveBookings()
        {
            return await context.Set<Booking>().Where(b => b.IsFinished != true).Include(a => a.Book).Include(a => a.User).ToListAsync();
        }

        public async Task<Booking> GetBookingById(int id)
        {
            return await context.Set<Booking>().Where(b => b.BookingId == id).Include(b => b.Book).Include(b => b.User).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookings()
        {
            return await context.Set<Booking>().Include(a => a.Book).Include(a => a.User).ToListAsync();
        }
    }
}
