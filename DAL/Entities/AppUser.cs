using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public bool isBlocked { get; set; } = false;
        [ForeignKey("IdentityRole<int>")]
        public int RoleId { get; set; }
    }
}
