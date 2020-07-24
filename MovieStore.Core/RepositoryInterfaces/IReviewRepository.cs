using MovieStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Core.RepositoryInterfaces
{
    public interface IReviewRepository:IAsyncRepository<Review>
    {
        
        Task<IEnumerable<Review>> GetReviewsById(int userId);
    }
}
