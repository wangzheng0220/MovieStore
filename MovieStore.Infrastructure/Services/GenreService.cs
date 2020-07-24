using Microsoft.Extensions.Caching.Memory;
using MovieStore.Core.Entities;
using MovieStore.Core.RepositoryInterfaces;
using MovieStore.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Infrastructure.Services
{
    public class GenreService : IGenreService
    {
        // In our application there might be some data that we get from database that wont change often??
        // Genres, in our application they dont change at all much
        // we display genres in our navigation header, GenreService will call GenreRepository, that will call Genre Table  --> databse call
        // we an use Im memoery caching to cache the list of genres for the first time...
        // next time when you try to get genres what you do is check if the cache has Genres lists, 
        // if yes then take that from cache if not go to databse and put them in cache
        
        private readonly IGenreRepository _genreRepository;
        private readonly IMemoryCache _memoryCache;
        private static readonly string _genresKey = "genres";
        private static readonly TimeSpan _defaultCacheDuration = TimeSpan.FromDays(30);

        // Constructor Injection, inject GenreRepository class instance
        public GenreService(IGenreRepository genreRepository, IMemoryCache memoryCache)
        {
            _genreRepository = genreRepository;
            _memoryCache = memoryCache;
        }
        public async Task<IEnumerable<Genre>> GetAllGenres()
        {
            // return await _genreRepository.ListAllAsync();
            // first check if the cache has genres by key

            var genres = await _memoryCache.GetOrCreateAsync(_genresKey, async entry =>
            {
                entry.SlidingExpiration = _defaultCacheDuration;
                return await _genreRepository.ListAllAsync();
            });

            return genres;
        }
    }
}
