using AutoMapper;

namespace BLL.Profiles
{
    class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<DAL.Entities.AppUser, Models.User>();
            CreateMap<Models.User, DAL.Entities.AppUser>();
        }
    }
}
