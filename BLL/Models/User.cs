using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
      //  public DateTime? DateOfBirth { get; set; }
     //   public ICollection<Booking> Bookings { get; set; }
    }
}
