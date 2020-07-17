﻿using MovieStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace MovieStore.Core.ServiceInterfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetTop25HighestRevenueMovies();
        Task<IEnumerable<Movie>> GetTop25RatedMovies();
        Task<Movie> GetMovieById(int id);
        Task<Movie> CreateMovie(Movie movie);
        Task<Movie> UpdateMovie(Movie movie);

        Task<int> GetMoviesCount(string title = "");
    }
}
