using MovieStore.Core.Entities;
using MovieStore.Core.RepositoryInterfaces;
using MovieStore.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Infrastructure.Repositories
{
    public class GenreRepository:EfRepository<Genre>, IGenreRepository
    {
        public GenreRepository(MovieStoreDbContext dbContext) : base(dbContext)
        {

        }
    }
}
