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
    public class BookServiceTests
    {
        List<Book> booksInMemoryDatabase;

        Mock<IUnitOfWork> _mockUnitOfWork;
        Mock<IMapper> _mockMapper;
        Mock<IBookRepository> _mockBookRepository;
        Mock<IUserRepository> _mockUserRepository;

        [TestInitialize]
        public void Initialize()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockBookRepository = new Mock<IBookRepository>();
            _mockUserRepository = new Mock<IUserRepository>();

            booksInMemoryDatabase = new List<Book>
            {
                new Book { BookId = 1, Name = "book1", BookYear = 1998, NumberAvailable = 3},
                new Book { BookId = 2, Name = "book2", BookYear = 2004, NumberAvailable = 5},
                new Book { BookId = 3, Name = "book3", BookYear = 1889, NumberAvailable = 1},
            };
        }

        [TestMethod]
        public async Task GetByIdAsync_ReturnsBook()
        {
            _mockBookRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
               .ReturnsAsync((int i) => booksInMemoryDatabase.Single(b => b.BookId == i));

            _mockMapper.Setup(x => x.Map<BLL.Models.Book>(It.IsAny<Book>())).Returns(new BLL.Models.Book { BookId = 3, BookYear = 1889, NumberAvailable = 1 });
            _mockUnitOfWork.Setup(x => x.BookRepository).Returns(_mockBookRepository.Object);

            var service = new BookService(_mockUnitOfWork.Object, _mockMapper.Object);

            var res = await service.GetByIdAsync(3);

            Assert.IsNotNull(res);
            Assert.AreEqual(1889, res.BookYear);
            Assert.AreEqual(1, res.NumberAvailable);
        }

        [TestMethod]
        [ExpectedException(typeof(AppException))]
        public async Task GetByIdAsync_Throws_IfBookIdIsInvalid()
        {
            _mockBookRepository.Setup(x => x.GetBookByIdAsync(It.IsAny<int>()))
               .ReturnsAsync((int i) => booksInMemoryDatabase.Single(b => b.BookId == i));

            _mockUnitOfWork.Setup(x => x.BookRepository).Returns(_mockBookRepository.Object);

            var service = new BookService(_mockUnitOfWork.Object, _mockMapper.Object);

            await service.GetByIdAsync(333);
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnsAllBooks()
        {
            _mockMapper.Setup(x => x.Map<IEnumerable<BLL.Models.Book>>(It.IsAny<IEnumerable<Book>>()))
                .Returns(new List<BLL.Models.Book>
                {
                    new BLL.Models.Book { BookId = 1, Name = "book1", BookYear = 1998, NumberAvailable = 3},
                    new BLL.Models.Book { BookId = 2, Name = "book2", BookYear = 2004, NumberAvailable = 5},
                    new BLL.Models.Book { BookId = 3, Name = "book3", BookYear = 1889, NumberAvailable = 1}
                 });

            _mockBookRepository.Setup(x => x.GetBooksAsync()).ReturnsAsync(booksInMemoryDatabase);
            _mockUnitOfWork.Setup(x => x.BookRepository).Returns(_mockBookRepository.Object);

            var service = new BookService(_mockUnitOfWork.Object, _mockMapper.Object);

            var res = await service.GetAllAsync();

            Assert.AreEqual(3, res.ToList().Count);
        }

        [TestMethod]
        public async Task AddAsync_AddsBook()
        {
            _mockUnitOfWork.Setup(x => x.BookRepository).Returns(_mockBookRepository.Object);

            _mockMapper.Setup(x => x.Map<BLL.Models.Book>(It.IsAny<Book>())).Returns(new BLL.Models.Book { BookId = 5, Name = "book5", NumberAvailable = 5 });
            _mockMapper.Setup(x => x.Map<Book>(It.IsAny<BLL.Models.Book>())).Returns(new Book { BookId = 5, Name = "book5", NumberAvailable = 5 });

            var service = new BookService(_mockUnitOfWork.Object, _mockMapper.Object);
            var book = new BLL.Models.Book { BookId = 5, Name = "book5", NumberAvailable = 5 };

            var res = await service.AddAsync(book);

            Assert.AreEqual(5, res.BookId);
            Assert.AreEqual("book5", res.Name);
            Assert.AreEqual(5, res.NumberAvailable);

        }

    }
}
