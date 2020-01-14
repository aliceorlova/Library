using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        public Book Book { get; set; }
        public AppUser User { get; set; }
        public bool IsFinished { get; set; }
        public DateTime DateOfBeginning { get; set; }
        public DateTime DateOfReturn { get; set; }
    }
}
