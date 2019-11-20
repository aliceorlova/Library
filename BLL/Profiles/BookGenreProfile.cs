using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

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
