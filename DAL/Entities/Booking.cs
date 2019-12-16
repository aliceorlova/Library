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
        public AppUser AppUser { get; set; }
        public bool IsFinished { get; set; }
        public DateTime DateOfBeginning { get; set; }
        public DateTime DateOfReturn { get; set; }
    }
}
