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
    public class GenreServiceTests
    {
        List<Genre> genresInMemoryDatabase;

        Mock<IUnitOfWork> _mockUnitOfWork;
        Mock<IMapper> _mockMapper;
        Mock<IGenreRepository> _mockRepository;

        [TestInitialize]
        public void Initialize()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IGenreRepository>();

            genresInMemoryDatabase = new List<Genre>
            {
                new Genre { GenreId = 1, Name = "Fiction"},
                new Genre { GenreId = 4, Name = "Novel"},
                new Genre { GenreId = 9, Name = "Satire"}
            };

            _mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int i) => genresInMemoryDatabase.Single(g => g.GenreId == i));

            _mockRepository.Setup(x => x.GetByName(It.IsAny<string>()))
                .ReturnsAsync((string name) => genresInMemoryDatabase.Single(a => a.Name == name));


            _mockUnitOfWork.Setup(x => x.GenreRepository).Returns(_mockRepository.Object);
        }

        [TestMethod]
        public async Task GetByIdAsync_ReturnsGenre()
        {
            _mockMapper.Setup(x => x.Map<BLL.Models.Genre>(It.IsAny<Genre>())).Returns(new BLL.Models.Genre { GenreId = 4, Name = "Novel" });

            var service = new GenreService(_mockUnitOfWork.Object, _mockMapper.Object);

            var res = await service.GetByIdAsync(4);

            Assert.AreEqual("Novel", res.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(AppException))]
        [DataRow(-2)]
        [DataRow(-155)]
        public async Task GetByIdAsync_Throws_IfIdIsInvalid(int id)
        {
            var service = new GenreService(_mockUnitOfWork.Object, _mockMapper.Object);

            await service.GetByIdAsync(id);
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnsGenres()
        {
            _mockMapper.Setup(x => x.Map<IEnumerable<BLL.Models.Genre>>(It.IsAny<IEnumerable<Genre>>()))
                .Returns(new List<BLL.Models.Genre>
                { new BLL.Models.Genre { GenreId = 4, Name = "Novel" },
                  new BLL.Models.Genre { GenreId = 4, Name = "Novel" },
                  new BLL.Models.Genre { GenreId = 4, Name = "Novel" } });

            _mockRepository.Setup(x => x.GetGenresAsync()).ReturnsAsync(genresInMemoryDatabase);

            var service = new GenreService(_mockUnitOfWork.Object, _mockMapper.Object);

            var res = await service.GetAllAsync();

            Assert.AreEqual(3, res.ToList().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(AppException))]
        public async Task AddAsync_Throws_IfGenreExists()
        {
           
            _mockRepository.Setup(x => x.GetByName(It.IsAny<string>())).ReturnsAsync(new Genre { });

            var service = new GenreService(_mockUnitOfWork.Object, _mockMapper.Object);
            var genre = new BLL.Models.Genre();

            await service.AddAsync(genre);
        }
    }
}
