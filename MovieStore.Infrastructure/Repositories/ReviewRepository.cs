using Microsoft.EntityFrameworkCore;
using MovieStore.Core.Entities;
using MovieStore.Core.RepositoryInterfaces;
using MovieStore.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Infrastructure.Repositories
{
    public class ReviewRepository:EfRepository<Review>, IReviewRepository
    {
        public ReviewRepository(MovieStoreDbContext dbContext) : base(dbContext)
        {

        }
       

        public async Task<IEnumerable<Review>> GetReviewsById(int userId)
        {
            var reviews = await _dbContext.Reviews.Include(r => r.User).Where(r => r.UserId == userId).ToListAsync();
            return reviews;

            //select * from Review
            //where UserId = 1889
        }
    }

       
   
}
