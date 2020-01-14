using AutoMapper;
 

namespace BLL.Profiles
{
    class BookGenreProfile : Profile
    {
        public BookGenreProfile()
        {
            CreateMap<DAL.Entities.BookGenre, Models.BookGenre>();
            CreateMap<Models.BookGenre, DAL.Entities.BookGenre>();
        }
    }
}
