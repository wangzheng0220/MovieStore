using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using MovieStore.Core.Entities;
using MovieStore.Infrastructure.Services;
using System.Threading.Tasks;
using MovieStore.Core.RepositoryInterfaces;
using System.Linq.Expressions;
using System.Linq;
using Moq;
namespace MovieStore.UnitTests.Services
{
    [TestFixture]
    public class MovieServiceTest
    {
        // SUT System Under Test
        private MovieService _sut;
        private Mock<IMovieRepository> _mockMovieRepository;
        private List<Movie> _movies;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _movies = new List<Movie> {
                new Movie {Id = 1, Title = "Avengers: Infinity War", Budget = 1200000},
                          new Movie {Id = 2, Title = "Avatar", Budget = 1200000},
                          new Movie {Id = 3, Title = "Star Wars: The Force Awakens", Budget = 1200000},
                          new Movie {Id = 4, Title = "Titanic", Budget = 1200000},
                          new Movie {Id = 5, Title = "Inception", Budget = 1200000},
                          new Movie {Id = 6, Title = "Avengers: Age of Ultron", Budget = 1200000},
                          new Movie {Id = 7, Title = "Interstellar", Budget = 1200000},
                          new Movie {Id = 8, Title = "Fight Club", Budget = 1200000},
                          new Movie
                          {
                              Id = 9, Title = "The Lord of the Rings: The Fellowship of the Ring", Budget = 1200000
                          },
                          new Movie {Id = 10, Title = "The Dark Knight", Budget = 1200000},
                          new Movie {Id = 11, Title = "The Hunger Games", Budget = 1200000},
                          new Movie {Id = 12, Title = "Django Unchained", Budget = 1200000},
                          new Movie
                          {
                              Id = 13, Title = "The Lord of the Rings: The Return of the King", Budget = 1200000
                          },
                          new Movie {Id = 14, Title = "Harry Potter and the Philosopher's Stone", Budget = 1200000},
                          new Movie {Id = 15, Title = "Iron Man", Budget = 1200000},
                          new Movie {Id = 16, Title = "Furious 7", Budget = 1200000}
            };
        }

        [SetUp]
        public void SetUp()
        {
            _mockMovieRepository = new Mock<IMovieRepository>();
            _mockMovieRepository.Setup(m => m.GetHighestRevenueMovies()).ReturnsAsync(_movies);
            _mockMovieRepository.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int m) => _movies.First(x => x.Id == m));

            _mockMovieRepository.Setup(m => m.GetTop25RatedMovies()).ReturnsAsync(_movies);
            
        }


        [Test]

        public async Task Test_Movie_Name_and_Budget_By_GivenMovie_Id()
        {
            _sut = new MovieService(_mockMovieRepository.Object);

            var movie = await _sut.GetByIdAsync(10);

            Assert.AreEqual("The Dark Knight", movie.Title);
            Assert.AreEqual(1200000, movie.Budget);
        }

        [Test]

        public async Task Test_MovieId_FromFakeData_That_Does_Not_Exist()
        {
            _sut = new MovieService(_mockMovieRepository.Object);

            // we can even check for any known and unknown exceptions
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _sut.GetMovieById(20));

          //  var movie = await _sut.GetByIdAsync(20);

        }


        // always make sure your method names are descriptive....
        [Test]
        public async Task Test_Top_25_Highest_RevenueMovies_FromFakeData()
        {
            // Unit testing should ideally not touch any external databases or resources, they should be isolated and tested independently
            // becasue in ur application you will have hundreds of mehtods that you need to be tested....if every method calls the dataabse, to run all those unit
            // tests it will take lots of time.
            // the purpose of unit test to to run as many as unit tests as possible very fast.
            // you might have 500 unit tests methods....
            // we always do our unit tests with in memory fake data....

            // AAA
            // Arrange, Act and Assert
            // Arrange: Initializes objects, creates mocks with arguments that are passed to the method under test and adds expectations. 
            _sut = new MovieService(_mockMovieRepository.Object);
            //   Act: Invokes the method or property under test with the arranged parameters.
            var movies = await _sut.GetTop25HighestRevenueMovies();

            //  Assert: Verifies that the action of the method under test behaves as expected.

            Assert.NotNull(movies);
            Assert.AreEqual(16, movies.Count());
            CollectionAssert.AllItemsAreInstancesOfType(movies, typeof(Movie));
          //  CollectionAssert.AreEquivalent( IEnumerable<Movie> , movies);

        }

        [Test]
        public async Task Test_Top_25_Rated_Movies_FromFakeData()
        {
            _sut = new MovieService(_mockMovieRepository.Object);
            var movies = await _sut.GetTop25RatedMovies();

            Assert.NotNull(movies);
            Assert.AreEqual(16, movies.Count());
            CollectionAssert.AllItemsAreInstancesOfType(movies, typeof(Movie));
        }

    }


   

}
