using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

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
