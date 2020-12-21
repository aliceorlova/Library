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
        Mock<IGenreRepository> _mockGenreRepository;
        Mock<IAuthorRepository> _mockAuthorRepository;

        [TestInitialize]
        public void Initialize()
        {
            _mockGenreRepository = new Mock<IGenreRepository>();
            _mockAuthorRepository = new Mock<IAuthorRepository>();

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
        public async Task GetAuthors_ReturnsExistingAuthor_WhenName()
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
