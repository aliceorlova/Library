using AutoMapper;
 

namespace BLL
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
