using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieStore.MVC.Models;

namespace MovieStore.MVC.Controllers
{
    public class MoviesController : Controller
    {

        //GET localhost/Movies

        [HttpGet]
        public IActionResult Index()
        {

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
            return View();
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
