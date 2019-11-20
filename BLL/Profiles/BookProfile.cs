using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

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
