using AutoMapper;
 
namespace BLL
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
