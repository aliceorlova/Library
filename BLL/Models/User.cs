using System.Collections.Generic;


namespace BLL.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool isBlocked { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}
