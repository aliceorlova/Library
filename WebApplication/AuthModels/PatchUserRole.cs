using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.AuthModels
{
    public class PatchUserRole
    {
        public int UserId { get; set; }
        public string Role { get; set; }
    }
}
