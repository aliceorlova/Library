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
    public class UnitOfWorkTests
    {
        List<Author> authorsInMemoryDatabase;
        List<Genre> genresInMemoryDatabase;
        List<Book> booksInMemoryDatabase;
        List<Booking> bookingsInMemoryDatabase;

        Mock<IGenreRepository> _mockGenreRepository;
        Mock<IAuthorRepository> _mockAuthorRepository;
        Mock<IBookingRepository> _mockBookingRepository;
        Mock<IBookRepository> _mockBookRepository;

        [TestInitialize]
        public void Initialize()
        {
            _mockGenreRepository = new Mock<IGenreRepository>();
            _mockAuthorRepository = new Mock<IAuthorRepository>();
            _mockBookingRepository = new Mock<IBookingRepository>();
            _mockBookRepository = new Mock<IBookRepository>();

            genresInMemoryDatabase = new List<Genre>
            {
                new Genre { GenreId = 1, Name = "Fiction"},
                new Genre { GenreId = 2, Name = "Science Fiction"},
                new Genre { GenreId = 3, Name = "Romance"},
                new Genre { GenreId = 4, Name = "Novel"},
                new Genre { GenreId = 9, Name = "Satire"}
            };

            authorsInMemoryDatabase = new List<Author>
            {
                new Author { AuthorId = 1, FirstName = "Thomas", LastName = "Hardy" },
                new Author { AuthorId = 2, FirstName = "George", LastName = "Orwell" },
                new Author { AuthorId = 3, FirstName = "Haruki", LastName = "Murakami" }
            };

            booksInMemoryDatabase = new List<Book>
            {
                new Book { BookId = 1, Name = "book1", BookYear = 1998, NumberAvailable = 3},
                new Book { BookId = 2, Name = "book2", BookYear = 2004, NumberAvailable = 5},
                new Book { BookId = 3, Name = "book3", BookYear = 1889, NumberAvailable = 1},
            };

            bookingsInMemoryDatabase = new List<Booking>
            {
                new Booking { BookingId = 1,  Book = booksInMemoryDatabase.First(b => b.BookId == 1),  User = new AppUser{ Id = 1 }, IsFinished = false },
                new Booking { BookingId = 2, Book = booksInMemoryDatabase.First(b => b.BookId == 2),  User = new AppUser{ Id = 2 }, IsFinished = false },
                new Booking { BookingId = 3, Book = booksInMemoryDatabase.First(b => b.BookId == 3), User = new AppUser { Id = 3 }, IsFinished = true }
            };
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public async Task GetBookingByIdAsync_ThrowsException_WhenBookingDoesNotExist()
        {
            _mockBookingRepository.Setup(x => x.GetBookingByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int i) => bookingsInMemoryDatabase.Single(b => b.BookingId == i));
            var uow = new UnitOfWork();
            uow.BookingRepository = _mockBookingRepository.Object;

            await uow.BookingRepository.GetBookingByIdAsync(11);
        }

        [TestMethod]
        [DataRow(1, 1)]
        [DataRow(2, 2)]
        [DataRow(3, 3)]
        public async Task GetBookingByIdAsync_ReturnsExistingBook_WhenIdIsValid(int id, int bookId)
        {
            _mockBookingRepository.Setup(x => x.GetBookingByIdAsync(It.IsAny<int>()))
                 .ReturnsAsync((int i) => bookingsInMemoryDatabase.Single(b => b.BookingId == i));
            var uow = new UnitOfWork();
            uow.BookingRepository = _mockBookingRepository.Object;

            var validBooking = await uow.BookingRepository.GetBookingByIdAsync(id);

            Assert.IsNotNull(validBooking);
            Assert.AreEqual(id, validBooking.Book.BookId);
        }

        [TestMethod]
        public async Task GetBookings_ReturnsExistingBookings()
        {
            _mockBookingRepository.Setup(x => x.GetBookingsAsync()).ReturnsAsync(bookingsInMemoryDatabase);
            var uow = new UnitOfWork();
            uow.BookingRepository = _mockBookingRepository.Object;

            var validBookings = await uow.BookingRepository.GetBookingsAsync();

            Assert.IsNotNull(validBookings);
            Assert.AreEqual(3, validBookings.ToList().Count);
        }

        [TestMethod]
        public async Task GetActiveBookings_ReturnsActiveBookings()
        {
            _mockBookingRepository.Setup(x => x.GetActiveBookingsAsync()).ReturnsAsync(bookingsInMemoryDatabase.Where(b => b.IsFinished == false));
            var uow = new UnitOfWork();
            uow.BookingRepository = _mockBookingRepository.Object;

            var activeBookings = await uow.BookingRepository.GetActiveBookingsAsync();

            Assert.IsNotNull(activeBookings);
            Assert.AreEqual(2, activeBookings.ToList().Count);
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public async Task GetBookByIdAsync_ThrowsException_WhenBookDoesNotExist()
        {
            _mockBookRepository.Setup(x => x.GetBookByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int i) => booksInMemoryDatabase.Single(b => b.BookId == i));
            var uow = new UnitOfWork();
            uow.BookRepository = _mockBookRepository.Object;

            await uow.BookRepository.GetBookByIdAsync(11);
        }

        [TestMethod]
        [DataRow(1, "book1")]
        [DataRow(2, "book2")]
        [DataRow(3, "book3")]
        public async Task GetBookByIdAsync_ReturnsExistingBook_WhenIdIsValid(int id, string name)
        {
            _mockBookRepository.Setup(x => x.GetBookByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int i) => booksInMemoryDatabase.Single(b => b.BookId == i));
            var uow = new UnitOfWork();
            uow.BookRepository = _mockBookRepository.Object;

            var validBook = await uow.BookRepository.GetBookByIdAsync(id);

            Assert.IsNotNull(validBook);
            Assert.AreEqual(id, validBook.BookId);
            Assert.AreEqual(name, validBook.Name);
        }

        [TestMethod]
        public async Task GetBooks_ReturnsExistingBooks()
        {
            _mockBookRepository.Setup(x => x.GetBooksAsync()).ReturnsAsync(booksInMemoryDatabase);
            var uow = new UnitOfWork();
            uow.BookRepository = _mockBookRepository.Object;

            var validBooks = await uow.BookRepository.GetBooksAsync();

            Assert.IsNotNull(validBooks);
            Assert.AreEqual(3, validBooks.ToList().Count);
        }

        [TestMethod]
        [DataRow(1, "Fiction")]
        [DataRow(9, "Satire")]
        [DataRow(3, "Romance")]
        public async Task GetGenreByIdAsync_ReturnsExistingGenre_WhenIdIsValid(int id, string name)
        {
            _mockGenreRepository.Setup(x => x.GetGenreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int i) => genresInMemoryDatabase.Single(g => g.GenreId == i));
            var uow = new UnitOfWork();
            uow.GenreRepository = _mockGenreRepository.Object;

            var validGenre = await uow.GenreRepository.GetGenreByIdAsync(id);

            Assert.IsNotNull(validGenre);
            Assert.AreEqual(id, validGenre.GenreId);
            Assert.AreEqual(name, validGenre.Name);
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public async Task GetGenreByIdAsync_ThrowsException_WhenGenreDoesNotExist()
        {
            _mockGenreRepository.Setup(x => x.GetGenreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int i) => genresInMemoryDatabase.Single(g => g.GenreId == i));
            var uow = new UnitOfWork();
            uow.GenreRepository = _mockGenreRepository.Object;

            await uow.GenreRepository.GetGenreByIdAsync(55);
        }

        [TestMethod]
        [DataRow(1, "Fiction")]
        [DataRow(2, "Science Fiction")]
        [DataRow(3, "Romance")]
        [DataRow(4, "Novel")]
        public async Task GetByName_ReturnsExistingGenre_WhenExists(int id, string name)
        {
            _mockGenreRepository.Setup(x => x.GetByName(It.IsAny<string>()))
                .ReturnsAsync((string name) => genresInMemoryDatabase.Single(a => a.Name == name));
            var uow = new UnitOfWork();
            uow.GenreRepository = _mockGenreRepository.Object;

            var validGenre = await uow.GenreRepository.GetByName(name);

            Assert.IsNotNull(validGenre);
            Assert.AreEqual(id, validGenre.GenreId);
            Assert.AreEqual(name, validGenre.Name);
        }

        [TestMethod]
        public async Task GetGenresAsync_ReturnsExistingGenres()
        {
            _mockGenreRepository.Setup(x => x.GetGenresAsync()).ReturnsAsync(genresInMemoryDatabase);
            var uow = new UnitOfWork();
            uow.GenreRepository = _mockGenreRepository.Object;

            var validGenres = await uow.GenreRepository.GetGenresAsync();

            Assert.IsNotNull(validGenres);
            Assert.AreEqual(5, validGenres.ToList().Count);
        }

        [TestMethod]
        public async Task GetAuthorByIdAsync_ReturnsExistingAuthor_WhenIdIs1()
        {
            _mockAuthorRepository.Setup(x => x.GetAuthorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int i) => authorsInMemoryDatabase.Single(a => a.AuthorId == i));
            var uow = new UnitOfWork();
            uow.AuthorRepository = _mockAuthorRepository.Object;

            var validAuthor = await uow.AuthorRepository.GetAuthorByIdAsync(1);

            Assert.IsNotNull(validAuthor);
            Assert.AreEqual(1, validAuthor.AuthorId);
            Assert.AreEqual("Thomas", validAuthor.FirstName);
            Assert.AreEqual("Hardy", validAuthor.LastName);
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public async Task GetAuthorByIdAsync_ThrowsException_WhenAuthorDoesNotExist()
        {
            _mockAuthorRepository.Setup(x => x.GetAuthorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int i) => authorsInMemoryDatabase.Single(a => a.AuthorId == i));
            var uow = new UnitOfWork();
            uow.AuthorRepository = _mockAuthorRepository.Object;

            await uow.AuthorRepository.GetAuthorByIdAsync(5);
        }

        [TestMethod]
        public async Task GetByName_ReturnsExistingAuthor_WhenName()
        {
            _mockAuthorRepository.Setup(x => x.GetByName(It.IsAny<string>(), It.IsAny<string>()))
               .ReturnsAsync((string first, string last) => authorsInMemoryDatabase.Single(a => a.FirstName == first && a.LastName == last));
            var uow = new UnitOfWork();
            uow.AuthorRepository = _mockAuthorRepository.Object;

            var validAuthor = await uow.AuthorRepository.GetByName("George", "Orwell");

            Assert.IsNotNull(validAuthor);
            Assert.AreEqual(2, validAuthor.AuthorId);
            Assert.AreEqual("George", validAuthor.FirstName);
            Assert.AreEqual("Orwell", validAuthor.LastName);
        }

        [TestMethod]
        public async Task GetAuthors_ReturnsExistingAuthors()
        {
            _mockAuthorRepository.Setup(x => x.GetAuthorsAsync()).ReturnsAsync(authorsInMemoryDatabase);
            var uow = new UnitOfWork();
            uow.AuthorRepository = _mockAuthorRepository.Object;

            var validAuthors = await uow.AuthorRepository.GetAuthorsAsync();

            Assert.IsNotNull(validAuthors);
            Assert.AreEqual(3, validAuthors.ToList().Count);
        }
    }
}
