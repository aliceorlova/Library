using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.AuthModels
{
    // the [Authorize] attribute requires roles to be passed as strings => not an enum
    public static class Role
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string User = "User";
    }
}
