using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    class Booking
    {
        public int BookingId { get; set; }
        public Book Book { get; set; }
        public User User { get; set; }
        public bool isFinished { get; set; }
    }
}
