using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    class AuthorProfile: Profile
    {
        public AuthorProfile()
        {
            CreateMap<DAL.Entities.Author,Models.Author>();
            CreateMap<Models.Author, DAL.Entities.Author>();
          //  CreateMap<IEnum>
        }
    }
}
