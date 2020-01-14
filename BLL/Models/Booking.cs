using System;

namespace BLL.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public Book Book { get; set; }
        public User User { get; set; }
        public bool isFinished { get; set; } = false;
        public DateTime DateOfBeginning { get; set; }
        public DateTime DateOfReturn { get; set; }

    }
}
