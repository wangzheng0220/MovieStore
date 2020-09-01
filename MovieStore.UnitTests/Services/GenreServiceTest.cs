using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using MovieStore.Core.Entities;
using MovieStore.Infrastructure.Services;
using System.Threading.Tasks;
using Moq;
using MovieStore.Core.RepositoryInterfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;

namespace MovieStore.UnitTests.Services
{
    [TestFixture]
    public class GenreServiceTest
    {
        private GenreService _sut;
        private Mock<IGenreRepository> _mockGenreRepository;
        private Mock<IMemoryCache> _mockMemoryCache;
        private List<Genre> _genres;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _genres = new List<Genre>
        {
            new Genre{Id = 1, Name = "Adventure"},
            new Genre{Id = 2, Name = "Fantasy"},
            new Genre{Id = 3, Name = "Animation"},
            new Genre{Id = 4, Name = "Drama"},
            new Genre{Id = 5, Name = "Horror"}
        };
        }

        [SetUp]
        public void SetUp()
        {
            _mockGenreRepository = new Mock<IGenreRepository>();
            _mockGenreRepository.Setup(g => g.ListAllAsync()).ReturnsAsync(_genres);
            _mockMemoryCache = new Mock<IMemoryCache>();
        }
        [Test]
        public async Task Test_List_All_Genres_FromFakeData()
        {
            // Arrange: Initializes objects, creates mocks with arguments that are passed to 
            //the method under test and adds expectations.
            _sut = new GenreService(_mockGenreRepository.Object,_mockMemoryCache.Object);
            //   Act: Invokes the method or property under test with the arranged parameters.
            var genres = await _sut.GetAllGenres();
            //  Assert: Verifies that the action of the method under test behaves as expected.
            Assert.NotNull(genres);
            Assert.AreEqual(5, genres.Count());
            CollectionAssert.AllItemsAreInstancesOfType(genres, typeof(Genre));
        }
    }

    
}
