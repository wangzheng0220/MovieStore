using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        
        //public virtual async Task<Movie> GetMovieById(int id)
        //{
        //    return await _dbContext.Set<Movie>().FindAsync(id);
        //}

        //public async Task<Movie> CreateMovie (Movie movie)
        //{
        //    await _dbContext.Set<Movie>().AddAsync(movie);
        //    await _dbContext.SaveChangesAsync();
        //    return movie;
        //}

        //public async Task<Movie> UpdateMovie (Movie movie)
        //{
        //    _dbContext.Entry(movie).State = EntityState.Modified;
        //    await _dbContext.SaveChangesAsync();
        //    return movie;
        //}
    }
}
