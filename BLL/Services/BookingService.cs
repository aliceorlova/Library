using AutoMapper;
using BLL.IServices;
using BLL.Models;
using DAL.UOW;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        IMapper _mapper;
        public BookingService(IUnitOfWork uow, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = uow;
        }

        public async Task<Booking> Add(Booking booking)
        {
            var bookDal = await _unitOfWork.BookRepository.GetBookById(booking.Book.BookId);
            var userDal = await _unitOfWork.UserRepository.GetById(booking.User.UserId);
            
            var b = new DAL.Entities.Booking { Book = bookDal, User = userDal, isFinished = false };
            Console.WriteLine("BOOKING" + b.BookingId);
          //  var bookingDal = _mapper.Map<DAL.Entities.Booking>(b);
            _unitOfWork.BookingRepository.Create(b);
            await _unitOfWork.Save();
            return _mapper.Map<Booking>(b);
        }

        public async Task<IEnumerable<Booking>> GetAll()
        {
            return _mapper.Map<IEnumerable<Booking>>(await _unitOfWork.BookingRepository.GetBookings());
        }

        public async Task<Booking> GetById(int id)
        {
            return _mapper.Map<Booking>(await _unitOfWork.BookingRepository.GetBookingById(id));
        }
    }
}
