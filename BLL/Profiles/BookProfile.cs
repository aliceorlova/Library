using AutoMapper;
 

namespace BLL.Profiles
{
    class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<DAL.Entities.Book, Models.Book>();
            CreateMap<Models.Book, DAL.Entities.Book>();
        }
    }
}
