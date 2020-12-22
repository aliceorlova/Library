using AutoMapper;
 
namespace BLL.Profiles
{
    class GenreProfile : Profile
    {
        public GenreProfile()
        {
            CreateMap<DAL.Entities.Genre, Models.Genre>();
            CreateMap<Models.Genre, DAL.Entities.Genre>();
        }
    }
}
