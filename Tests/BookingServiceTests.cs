using AutoMapper;
using BLL;
using BLL.Services;
using DAL.Entities;
using DAL.IRepositories;
using DAL.UOW;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class BookingServiceTests
    {
        List<Booking> bookingsInMemoryDatabase;

        Mock<IUnitOfWork> _mockUnitOfWork;
        Mock<IMapper> _mockMapper;
        Mock<IBookingRepository> _mockRepository;
        Mock<IBookRepository> _mockBookRepository;
        Mock<IUserRepository> _mockUserRepository;

        [TestInitialize]
        public void Initialize()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IBookingRepository>();
            _mockBookRepository = new Mock<IBookRepository>();
            _mockUserRepository = new Mock<IUserRepository>();

            bookingsInMemoryDatabase = new List<Booking>
            {
                new Booking { BookingId  = 1, Book = new Book{BookId = 6}, User = new AppUser{ Id = 4 }, IsFinished = false, DateOfBeginning = new System.DateTime(2020, 01, 16) },
                new Booking { BookingId  = 2, Book = new Book{BookId = 4}, User = new AppUser{ Id = 3 }, IsFinished = true, DateOfBeginning = new System.DateTime(2020, 01, 16) },
                new Booking { BookingId  = 3, Book = null, User = new AppUser{ Id = 2 }, IsFinished = false, DateOfBeginning = new System.DateTime(2020, 01, 16) }
            };
        }

        [TestMethod]
        public async Task GetByIdAsync_ReturnsBooking()
        {
            _mockRepository.Setup(x => x.GetBookingByIdAsync(It.IsAny<int>()))
               .ReturnsAsync((int i) => bookingsInMemoryDatabase.Single(b => b.BookingId == i));

            _mockMapper.Setup(x => x.Map<BLL.Models.Booking>(It.IsAny<Booking>())).Returns(new BLL.Models.Booking { BookingId = 5, isFinished = false, Book = new BLL.Models.Book { BookId = 4 }, User = new BLL.Models.User { UserId = 2 }, DateOfBeginning = new System.DateTime(2020, 01, 16), DateOfReturn = new System.DateTime(2020, 02, 16) });
            _mockUnitOfWork.Setup(x => x.BookingRepository).Returns(_mockRepository.Object);

            var service = new BookingService(_mockUnitOfWork.Object, _mockMapper.Object);

            var res = await service.GetByIdAsync(3);

            Assert.IsNotNull(res);
            Assert.AreEqual(false, res.isFinished);
            Assert.AreEqual(2, res.User.UserId);
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnsAllBookings()
        {
            _mockMapper.Setup(x => x.Map<IEnumerable<BLL.Models.Booking>>(It.IsAny<IEnumerable<Booking>>()))
                .Returns(new List<BLL.Models.Booking>
                {
                    new BLL.Models.Booking { BookingId  = 1, Book = new BLL.Models.Book{BookId = 6}, User = new BLL.Models.User{ UserId = 4 }, isFinished = false, DateOfBeginning = new System.DateTime(2020, 01, 16)},
                    new BLL.Models.Booking { BookingId  = 2, Book = new BLL.Models.Book {BookId = 4}, User = new BLL.Models.User{ UserId = 3 }, isFinished = true, DateOfBeginning = new System.DateTime(2020, 01, 16) },
                    new BLL.Models.Booking { BookingId  = 3, Book = null, User = new BLL.Models.User{ UserId = 2 }, isFinished = false, DateOfBeginning = new System.DateTime(2020, 01, 16) }
                 });

            _mockRepository.Setup(x => x.GetBookingsAsync()).ReturnsAsync(bookingsInMemoryDatabase);
            _mockUnitOfWork.Setup(x => x.BookingRepository).Returns(_mockRepository.Object);

            var service = new BookingService(_mockUnitOfWork.Object, _mockMapper.Object);

            var res = await service.GetAllAsync();

            Assert.AreEqual(3, res.ToList().Count);
        }

        [TestMethod]
        public async Task AddAsync_AddsBooking()
        {
            _mockBookRepository.Setup(x => x.GetBookByIdAsync(It.IsAny<int>())).ReturnsAsync(new Book { NumberAvailable = 1 });
            _mockUserRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new AppUser { Bookings = { new Booking { } }, isBlocked = false });
            _mockRepository.Setup(x => x.GetBookingByIdAsync(It.IsAny<int>())).ReturnsAsync(new Booking { });

            _mockUnitOfWork.Setup(x => x.BookingRepository).Returns(_mockRepository.Object);
            _mockUnitOfWork.Setup(x => x.BookRepository).Returns(_mockBookRepository.Object);
            _mockUnitOfWork.Setup(x => x.UserRepository).Returns(_mockUserRepository.Object);

            var service = new BookingService(_mockUnitOfWork.Object, _mockMapper.Object);

            var booking = new BLL.Models.Booking { BookingId = 5, Book = new BLL.Models.Book { BookId = 4 }, User = new BLL.Models.User { UserId = 2 } };
            _mockMapper.Setup(x => x.Map<BLL.Models.Book>(It.IsAny<Book>())).Returns(new BLL.Models.Book { NumberAvailable = booking.Book.NumberAvailable });

            var res = await service.AddAsync(booking);

            Assert.AreEqual(new DateTime(2020, 12, 21), res.DateOfBeginning);
            Assert.AreEqual(new DateTime(2020, 12, 21).AddDays(30), res.DateOfReturn);
            Assert.AreEqual(0, res.Book.NumberAvailable);

        }

    }
}
