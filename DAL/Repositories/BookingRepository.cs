using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories
{
   class BookingRepository: Repository<Booking>, IBookingRepository
    {
        public BookingRepository(AppContext context) : base(context) { }

    }
}
