using Microsoft.EntityFrameworkCore;
using MovieStore.Core.Entities;
using MovieStore.Core.RepositoryInterfaces;
using MovieStore.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MovieStore.Infrastructure.Repositories
{
    public class UserRepository : EfRepository<User>, IUserRepository
    {
        public UserRepository(MovieStoreDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async override Task<User> GetByIdAsync(int id)
        {
            return await _dbContext.Users.Include(u => u.Purchases).ThenInclude(m=>m.Movie).ThenInclude(f=>f.Favorites).Where(u => u.Id == id).FirstOrDefaultAsync();
                //userId 
        }

        public async Task<IEnumerable<int>> GetPurchasedMovieIdByUserId(int userId)
        {
            var PurchasedMovieId = await (from u in _dbContext.Users
                                          join p in _dbContext.Purchases
                                          on u.Id equals p.UserId
                                          where p.UserId == userId
                                          select p.MovieId).ToListAsync();
            return PurchasedMovieId;
            //select p.MovieId from[User] as u inner join Purchase as p on u.Id = p.Id
            //where p.UserId = u.Id
            //group by p.MovieId
        }
    }
}
