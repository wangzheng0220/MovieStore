using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Core.ServiceInterfaces;
using MovieStore.Infrastructure.Services;
using MovieStore.MVC.Models;

namespace MovieStore.MVC.Controllers
{
    public class MoviesController : Controller
    {
        // IOC, ASP.NET Core has build-in IOC/DI <INTERVIEW QUESTION>
        // In .NET Framework we need to reply on third-party IOC to do Dependency Injection, Ninject
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        //GET localhost/Movies

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // call our Movie Service, method, highest grossing method
            //var movies = await _movieService.GetTop25HighestRevenueMovies();
            //var movies = await _movieService.GetTop25RatedMovies();
            //var movies = await _movieService.GetMovieById(15);
            //var movies = await _movieService.GetMoviesCount("Iron Man");
            var movies = await _movieService.GetAverageRatedMovie(15);
            return View(movies);





            // public async Task<IActionResult> Index()
            //t1
            // var movieService = new MovieService();
            // 5 seconds var movies = await movieService.GetAllMovie(); i/0 bound operation
            // return a Task for you

            // Improving the scalability of the application, so that your application can server many concurrent requests properly
            // async/await will prevent thread starvation scenario.
            // recommand I/O bound operation not CPU
            // Database calls, Http calls, over network
            // Task<Movie>, Task<int>
            // in your C# or any library whenever you see a method with Async in the method name, that means you can await that method
            // EF, two kind of methods, normal sync method, async method...


            // go to database and get some list of movies and give it to the view
            // 

            //var movies = new List<Movie>
            //{
            //    new Movie {Id = 1, Title = "Avengers: Infinity War", Budget = 1200000},
            //    new Movie {Id = 2, Title = "Avatar", Budget = 1200000},
            //    new Movie {Id = 3, Title = "Star Wars: The Force Awakens", Budget = 1200000},
            //    new Movie {Id = 4, Title = "Titanic", Budget = 1200000},
            //    new Movie {Id = 5, Title = "Inception", Budget = 1200000},
            //    new Movie {Id = 6, Title = "Avengers: Age of Ultron", Budget = 1200000},
            //    new Movie {Id = 7, Title = "Interstellar", Budget = 1200000},
            //    new Movie {Id = 8, Title = "Fight Club", Budget = 1200000},
            //};

            //ViewBag.MoviesCount = movies.Count;
            //ViewData["myname"] = "John Doe";
            // compile time checks vs run-time checks


            // we need to pass data from Controller action method to the View
            // Usually its prefererrd to send a strongly typed Model or object to the View

            //3 ways to send data from Controller to View
            //1. Strongly-typed models (preferred way)
            //2. ViewBag --dynamic
            //3. ViewData - key/value
            //return View();
        }

        [HttpGet]
        [Route("Movies/Genre/{genreId}")] //{}变量 URL 对应上 用
        public async Task<IActionResult> Genre(int genreId)
        {
            var movies = await _movieService.GetMoviesByGenre(genreId);
            return View(movies); //pass data from controller to view
        }


        [HttpGet]
        [Route("Movies/Details/{movieId}")]
        public async Task<IActionResult> Details(int movieId)
        {
            var movie = await _movieService.GetByIdAsync(movieId);
            //decimal rating = await _movieService.GetAverageRatedMovie(movieId);
            return View(movie);
        }

        [HttpPost]
        public IActionResult Create(string title, decimal budget)
        {
            //POST// http:localhost/Movies/create

            //Model binding they are case in-sensitive
            //look at in-coming request and maps the input elements name/value with the parameter names of the action method
            //then the parameter will have the value automatically
            //it will also does casting/converting

            // we need to get the data from the view and save it in database
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            //GET// http:localhost/Movies/create
            // we need to have this method so that we can show the empty page for user to enter Movie information that needs to be created
            return View();
        }
        
    }
}
