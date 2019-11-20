using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Entities
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        public Book Book { get; set; }
        public User User { get; set; }
        public bool isFinished { get; set; }
    }
}
