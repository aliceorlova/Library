using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.AuthModels;

namespace WebApplication
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<BLL.Models.User, UserModel>();
            CreateMap<RegisterModel, BLL.Models.User>();
            CreateMap<UpdateModel, BLL.Models.User>();
            CreateMap<UserModel, BLL.Models.User>();
        }
    }
}
