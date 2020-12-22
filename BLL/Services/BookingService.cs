using AutoMapper;
using BLL.IServices;
using BLL.Models;
using DAL.UOW;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class BookingService : IBookingService
    {
        readonly IUnitOfWork _unitOfWork;
        IMapper _mapper;
        public BookingService(IUnitOfWork uow, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = uow;
        }

        public async Task<Booking> AddAsync(Booking booking)
        {
            var bookDal = await _unitOfWork.BookRepository.GetBookByIdAsync(booking.Book.BookId);
            if (bookDal.NumberAvailable == 0) throw new AppException("No books available.");
            var bookings = await _unitOfWork.BookingRepository.GetBookingsAsync();
         //   var res = bookings.Where(b => b.User.Id == booking.User.Id).ToList().Count;
            var userDal = await _unitOfWork.UserRepository.GetByIdAsync(booking.User.UserId);

            if (userDal.Bookings.Count > 5) throw new AppException("Too many books.");
            if (userDal.isBlocked == true) throw new AppException("User is blocked.");
            var b = new DAL.Entities.Booking {
                Book = bookDal, 
                User = userDal, 
                IsFinished = false, 
                DateOfBeginning = DateTime.Today, 
                DateOfReturn = DateTime.Today.AddDays(30) 
            };
            bookDal.NumberAvailable--;
            userDal.Bookings.Add(b);
            
            _unitOfWork.BookingRepository.Create(b);
            await _unitOfWork.SaveAsync();
            var result = new Booking { BookingId = b.BookingId, Book = _mapper.Map<Book>(b.Book), User = _mapper.Map<User>(b.User), DateOfBeginning = b.DateOfBeginning, DateOfReturn = b.DateOfReturn, isFinished = b.IsFinished };
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            _unitOfWork.BookingRepository.Delete(await _unitOfWork.BookingRepository.GetByIdAsync(id));
            await _unitOfWork.SaveAsync();
        }

        public async Task<Booking> FinishBookingAsync(int id)
        {
            var bookingDal = await _unitOfWork.BookingRepository.GetBookingByIdAsync(id);
            if (bookingDal == null) throw new AppException("The booking you are trying to update does not exist.");
            var user = await _unitOfWork.UserRepository.GetByIdAsync(bookingDal.User.Id);
            if (user == null) throw new AppException("USER NULL");

            var book = await _unitOfWork.BookRepository.GetBookByIdAsync(bookingDal.Book.BookId);
            if (book == null) throw new AppException("BOOK NULL");

            bookingDal.DateOfReturn = DateTime.Today;
            bookingDal.IsFinished = true;
            //if (bookingDal.DateOfReturn.CompareTo(DateTime.Today) > 0)
            //{
            //    user.isBlocked = true;
            //}
            book.NumberAvailable++;
            user.Bookings.Remove(bookingDal);
            _unitOfWork.BookingRepository.Update(bookingDal);
            await _unitOfWork.SaveAsync();
            var result = _mapper.Map<Booking>(bookingDal);
            result.User = _mapper.Map<User>(user);
            result.User.UserId = user.Id;
            return result;
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<Booking>>(await _unitOfWork.BookingRepository.GetBookingsAsync());
        }

        public async Task<IEnumerable<Booking>> GetAllActiveAsync()
        {
            return _mapper.Map<IEnumerable<Booking>>(await _unitOfWork.BookingRepository.GetActiveBookingsAsync());
        }

        public async Task<Booking> GetByIdAsync(int id)
        {
            return _mapper.Map<Booking>(await _unitOfWork.BookingRepository.GetBookingByIdAsync(id));
        }
        // can i change ids of user and book ?
        public async Task UpdateAsync(int id, Booking booking)
        {
            var existing = await _unitOfWork.BookingRepository.GetByIdAsync(id);
            if (existing == null) throw new AppException("The booking you are trying to update does not exist.");
            existing.User = _mapper.Map<DAL.Entities.AppUser>(booking.User);
            existing.Book = _mapper.Map<DAL.Entities.Book>(booking.Book);
            //  existing.IsFinished
            _unitOfWork.BookingRepository.Update(existing);
            await _unitOfWork.SaveAsync();
        }
    }
}
