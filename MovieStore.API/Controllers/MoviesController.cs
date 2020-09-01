using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Core.Entities;
using MovieStore.Core.ServiceInterfaces;

namespace MovieStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        // we want to construct a URL for showing top 25 revenue movies
        // [Route("api/[controller]")]
        // http:localhost/api/movies/toprevenue -- GET
        // SEO , RESTFul URL's, -- should be human readable

        [HttpGet]
        [Route("toprevenue")]
        public async Task<IActionResult> GetTopRevenueMovies ()
        {
            var movies = await _movieService.GetTop25HighestRevenueMovies();
            // in MVC we return Views
            // return data along with HTTP Status CODE
            //  OK -200
            if (!movies.Any())
            {
                return NotFound("No Movies Found!");
            }
            return Ok(movies);
        }

        [HttpGet]
        [Route("genre/{genreId:int}")]
        public async Task<IActionResult> GetMovieByGenre(int genreId)
        {
            var movies = await _movieService.GetMoviesByGenre(genreId);
            return Ok(movies);
        }

        [HttpGet]
        [Route("details/{movieId:int}")]
        public async Task<IActionResult> ShowMovieDetails(int movieId)
        {
            
            var movieDetails = await _movieService.GetByIdAsync(movieId);
            return Ok(movieDetails);
        }
    }
}
