using AutoMapper;
 

namespace BLL.Profiles
{
    class AuthorProfile: Profile
    {
        public AuthorProfile()
        {
            CreateMap<DAL.Entities.Author,Models.Author>();
            CreateMap<Models.Author, DAL.Entities.Author>();
        }
    }
}
