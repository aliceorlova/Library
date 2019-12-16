using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.AuthModels;

namespace WebApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<BLL.Models.User, UserModel>();
            CreateMap<RegisterModel, BLL.Models.User>();
            CreateMap<UpdateModel, BLL.Models.User>();
            CreateMap<UserModel, BLL.Models.User>();
            CreateMap<AuthenticateModel, BLL.Models.User>();
        }
    }
}
