using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieStore.Core.ServiceInterfaces;
using MovieStore.MVC.Models;

namespace MovieStore.MVC.Controllers
{
    //Any C# class can become a MVC controller if it inherits from Controller base class from Microsoft.AspNetCore.Mvc

    //http://localhost:2323/Home/index/2

    //http://localhost:2323/Home/index/0
    //http://localhost:2323/Home/index
    // Routing -- Pattern matching technique
    //HomeController
    // Index -- Action method
    public class HomeController : Controller
    {
        private readonly IMovieService _movieService;

        public HomeController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        // Action method
        public async Task< IActionResult> Index()
        {


            // we need or Movie Card we are going to use that one in lots of places...
            // 1. Home page -- show top revenue movies --> Movie Card
            // 2. Genres/show Movies belonginf to that genre --> Movie Card
            // 3. Top Rated Movies --> Top Movies as a card
            // We have to create this Movie Card in such a way that it can be reused in lots of places
            // Partial Views will help us in creating reusable Views across the application
            // Partial views are views inside another view

            // 0 and null are not equal
            // return a instance of a class that implements that IActionResult
            // By default MCV will look for same View name as Action method name
            // it will look inside Views folder --> Home (same name as Controller) --> index.

            // 1. Program.cs --> Main method
            // 2. StartUp Class
            // 3. ConfigureServices
            // 4. Configure
            // 5. HomeController
            // 6. Action method
            // 7. return a View

            // In ASP.NET Core Middleware.....a piece of software logic that will be executed...
            // Train --- DC to Boston
            // DC ===20, multiple stops... Phili, NJ, NY -- Boston
            // request --> M1-- some process--> next M2 --> next M3..M4..M5 --> Response

            var movies = await _movieService.GetTop25HighestRevenueMovies();
            return View(movies); //pass data from controller to view
        }
        
        public interface XYX
        {
            int Add(int x, int y);

        }

        public class MyClass : XYX
        {
            public int Add(int x, int y)
            {
                return x + y;
            }
        }

        public class MyClass2 : XYX
        {
            public int Add(int x, int y)
            {
                return x + y;
            }
        }
    }
}
