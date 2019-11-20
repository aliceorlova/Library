using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

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
