using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Profiles
{
    class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<DAL.Entities.User, Models.User>();
            CreateMap<Models.User, DAL.Entities.User>();
        }
    }
}
