using MovieStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStore.Core.RepositoryInterfaces
{
    public interface IFavoriteRepository:IAsyncRepository<Favorite>
    {
    }
}
