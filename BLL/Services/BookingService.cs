﻿using AutoMapper;
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
            if (userDal.Bookings.Count > 5) throw new AppException("Too many books.");
            if (userDal.isBlocked == true) throw new AppException("User is blocked.");
            var b = new DAL.Entities.Booking { Book = bookDal, User = userDal, isFinished = false };
            

            _unitOfWork.BookingRepository.Create(b);
            await _unitOfWork.Save();
            return _mapper.Map<Booking>(b);
        }

        public async Task Delete(int id)
        {
            _unitOfWork.BookingRepository.Delete(await _unitOfWork.BookingRepository.GetById(id));
            await _unitOfWork.Save();
        }

        public async Task<IEnumerable<Booking>> GetAll()
        {
            return _mapper.Map<IEnumerable<Booking>>(await _unitOfWork.BookingRepository.GetBookings());
        }

        public async Task<Booking> GetById(int id)
        {
            return _mapper.Map<Booking>(await _unitOfWork.BookingRepository.GetBookingById(id));
        }
        // can i change ids of user and book ?
        public async Task Update(int id, Booking booking)
        {
            var existing = await _unitOfWork.BookingRepository.GetById(id);
            if (existing == null) throw new AppException("The booking you are trying to update does not exist.");
            existing.User = _mapper.Map<DAL.Entities.User>(booking.User);
            existing.Book = _mapper.Map<DAL.Entities.Book>(booking.Book);

            _unitOfWork.BookingRepository.Update(existing);
            await _unitOfWork.Save();
        }
    }
}