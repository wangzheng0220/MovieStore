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
    public class FavoriteRepository : EfRepository<Favorite>, IFavoriteRepository
    {
        public FavoriteRepository(MovieStoreDbContext dbContext) : base(dbContext)
        {

        }

        public async override Task<Favorite> GetByIdAsync(int id)
        {
            return await _dbContext.Favorites.Include(f => f.User).Where(f => f.UserId == id).FirstOrDefaultAsync();
        }
    }
}
