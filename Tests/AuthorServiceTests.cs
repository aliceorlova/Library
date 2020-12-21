using AutoMapper;
using BLL;
using BLL.Services;
using DAL.Entities;
using DAL.IRepositories;
using DAL.UOW;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class AuthorServiceTests
    {
        List<Author> authorsInMemoryDatabase;

        Mock<IUnitOfWork> _mockUnitOfWork;
        Mock<IMapper> _mockMapper;
        Mock<IAuthorRepository> _mockRepository;

        [TestInitialize]
        public void Initialize()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IAuthorRepository>();

            authorsInMemoryDatabase = new List<Author>
            {
                new Author { AuthorId = 1, FirstName = "George", LastName = "Orwell"},
                new Author { AuthorId = 2, FirstName = "Thomas", LastName = "Hardy"},
                new Author { AuthorId = 3, FirstName = "Paulo", LastName = "Coelho"}
            };

            _mockRepository.Setup(x => x.GetAuthorByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int i) => authorsInMemoryDatabase.Single(a => a.AuthorId == i));

            _mockRepository.Setup(x => x.GetByName(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string first, string last) => authorsInMemoryDatabase.Single(a => a.FirstName == first && a.LastName == last));
        }

        [TestMethod]
        public async Task GetByIdAsync_ReturnsAuthor()
        {
            _mockMapper.Setup(x => x.Map<BLL.Models.Author>(It.IsAny<Author>())).Returns(new BLL.Models.Author { AuthorId = 3, FirstName = "Paulo", LastName = "Coelho" });
            _mockUnitOfWork.Setup(x => x.AuthorRepository).Returns(_mockRepository.Object);

            var service = new AuthorService(_mockUnitOfWork.Object, _mockMapper.Object);

            var res = await service.GetByIdAsync(3);

            Assert.AreEqual("Coelho", res.LastName);
            Assert.AreEqual("Paulo", res.FirstName);
            Assert.AreEqual(3, res.AuthorId);
        }

        [TestMethod]
        [ExpectedException(typeof(AppException))]
        [DataRow(-2)]
        [DataRow(-155)]
        public async Task GetByIdAsync_Throws_IfIdIsInvalid(int id)
        {
            var service = new AuthorService(_mockUnitOfWork.Object, _mockMapper.Object);

            await service.GetByIdAsync(id);
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnsAuthorss()
        {
            _mockMapper.Setup(x => x.Map<IEnumerable<BLL.Models.Author>>(It.IsAny<IEnumerable<Author>>()))
                .Returns(new List<BLL.Models.Author>
                { new BLL.Models.Author { AuthorId = 1, FirstName = "George", LastName = "Orwell"},
                  new BLL.Models.Author { AuthorId = 2, FirstName = "Thomas", LastName = "Hardy"},
                  new BLL.Models.Author { AuthorId = 3, FirstName = "Paulo", LastName = "Coelho"} });

            _mockRepository.Setup(x => x.GetAuthorsAsync()).ReturnsAsync(authorsInMemoryDatabase);
            _mockUnitOfWork.Setup(x => x.AuthorRepository).Returns(_mockRepository.Object);

            var service = new AuthorService(_mockUnitOfWork.Object, _mockMapper.Object);

            var res = await service.GetAllAsync();

            Assert.AreEqual(3, res.ToList().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(AppException))]
        public async Task AddAsync_Throws_IfGenreExists()
        {
            _mockRepository.Setup(x => x.GetByName(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new Author { });
            _mockMapper.Setup(x => x.Map<Author>(It.IsAny<BLL.Models.Author>())).Returns(new Author { });
            _mockUnitOfWork.Setup(x => x.AuthorRepository).Returns(_mockRepository.Object);

            var service = new AuthorService(_mockUnitOfWork.Object, _mockMapper.Object);
            var author = new BLL.Models.Author();

            await service.AddAsync(author);
        }
    }
}
