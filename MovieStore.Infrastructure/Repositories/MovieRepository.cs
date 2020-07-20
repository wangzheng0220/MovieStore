using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MovieStore.Core.Entities;
using MovieStore.Core.RepositoryInterfaces;
using MovieStore.Infrastructure.Data;

namespace MovieStore.Infrastructure.Repositories
{
    public class MovieRepository : EfRepository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieStoreDbContext dbContext) : base(dbContext) //调用base class consturctor
        {
        }

        public async Task<IEnumerable<Movie>> GetHighestRevenueMovies()
        {
            var movies = await _dbContext.Movies.OrderByDescending(m => m.Revenue).Take(25).ToListAsync();
            // select top 25 from Movies order by Revenue desc;
            return movies;
        }

        public async Task<IEnumerable<Movie>> GetTop25RatedMovies()
        {
            //var movies = await _dbContext.Movies.OrderByDescending(m => m.Rating).Take(25).ToListAsync();
            var movies = await _dbContext.Movies.OrderByDescending(m => m.Reviews.Average(r => r.Rating)).Take(25).ToListAsync();
            //           select top 25 m.Id from movie m left join review r on m.id = r.movieid
            //group by m.Id order by avg(r.rating) desc
            //m => m.Reviews.Average(r=>r.Rating)
            return movies;
        }

        public async Task<IEnumerable<Movie>> GetMoviesByGenre(int genreId)
        {

            //var movies = await _dbContext.Movies.Include(g=>g.MovieGenres).Where(w=>w.MovieGenres.)
            var movies = await (from m in _dbContext.Movies
                          join mg in _dbContext.MovieGenres on m.Id equals mg.MovieId
                          where mg.GenreId == genreId
                          select m).ToListAsync();
            return movies;

            
            //select* from Movie m inner join MovieGenre mg on m.Id = mg.MovieId
            //where GenreId = 2
        }

        public async Task<decimal> GetAverageRatedMovie(int movieId)
        {
            //decimal rating = await _dbContext.Movies.AverageAsync(m => m.Reviews.Average(r => r.Rating))
            //return rating;

            decimal rating = await (from m in _dbContext.Movies
                              join r in _dbContext.Reviews on m.Id equals r.MovieId
                              where m.Id == movieId
                              select r.Rating).AverageAsync();
           
            return rating;
            //select avg(r.Rating), r.MovieId from Movie as m inner join Review as r on m.Id= r.MovieId
            // where r.MovieId = 1
            //group by r.MovieId
        }




        public override async Task<Movie> GetByIdAsync(int id)
        {
            var movie = await _dbContext.Movies.Include(m => m.MovieCasts).ThenInclude(c => c.Cast)
                .FirstOrDefaultAsync(m => m.Id == id);
            movie.Rating = await GetAverageRatedMovie(id);
            return movie;
        }
    }
}
