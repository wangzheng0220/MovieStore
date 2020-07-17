using MovieStore.Core.Entities;
using MovieStore.Core.RepositoryInterfaces;
using MovieStore.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Infrastructure.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        // Constructor Injection, inject MovieRepository class instance
        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }
        public async Task<IEnumerable<Movie>> GetTop25HighestRevenueMovies()
        {
            return await _movieRepository.GetHighestRevenueMovies();
        }

        public async Task<IEnumerable<Movie>> GetTop25RatedMovies()
        {
            return await _movieRepository.GetTop25RatedMovies();
        }

        public async Task<Movie> GetMovieById(int id)
        {
            return await _movieRepository.GetByIdAsync(id);
        }
        public async Task<Movie> CreateMovie(Movie movie)
        {
            return await _movieRepository.AddAsync(movie);
        }

        public async Task<Movie> UpdateMovie(Movie movie)
        {
            return await _movieRepository.UpdateAsync(movie);
        }

        public async Task<int> GetMoviesCount(string title="")
        {
            return await _movieRepository.GetCountAsync(gmc=>gmc.Title==title);
            //<Func<T, bool>> filter = null);
        }
    }
    // Dependency Injection
    //public class MovieServiceTest : IMovieService // show lose coupling, change code in configureService, AddScoped
    //{
    //    public async Task<IEnumerable<Movie>> GetTop25HighestRevenueMovies()
    //    {
    //        var movies = new List<Movie>()
    //                    {
    //                        new Movie {Id = 1, Title = "Avengers: Infinity War", Budget = 1200000},
    //                        new Movie {Id = 2, Title = "Avatar", Budget = 1200000},
    //                        new Movie {Id = 3, Title = "Star Wars: The Force Awakens", Budget = 1200000},
    //                        new Movie {Id = 4, Title = "Titanic", Budget = 1200000},
    //                    };
    //        return movies;
    //    }
    //}
}
