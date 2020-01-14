using AutoMapper;
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
