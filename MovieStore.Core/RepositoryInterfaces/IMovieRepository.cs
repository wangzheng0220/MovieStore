using MovieStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Core.RepositoryInterfaces
{
    public interface IMovieRepository:IAsyncRepository<Movie>
    {
        Task<IEnumerable<Movie>> GetHighestRevenueMovies();

        Task<IEnumerable<Movie>> GetTop25RatedMovies();
    }


    // IAsyncRepos has 8 methods
    // public class MovieRepo: EfRepository, IMovieRepository
    // {
    //   1 + 8 // 1 method
    // }
}
