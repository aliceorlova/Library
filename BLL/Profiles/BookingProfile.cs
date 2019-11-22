using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Profiles
{
    class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<DAL.Entities.Booking, Models.Booking>();
            CreateMap<Models.Booking, DAL.Entities.Booking>();
        }

    }
}
