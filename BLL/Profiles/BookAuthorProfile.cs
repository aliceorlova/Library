using AutoMapper;
 

namespace BLL.Profiles
{
    class BookAuthorProfile : Profile
    {
        public BookAuthorProfile()
        {
            CreateMap<DAL.Entities.BookAuthor, Models.BookAuthor>();
            CreateMap<Models.BookAuthor, DAL.Entities.BookAuthor>();
        }
    }
}
