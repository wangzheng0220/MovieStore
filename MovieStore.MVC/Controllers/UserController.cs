using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MovieStore.Core.Models.Request;
using MovieStore.Core.RepositoryInterfaces;
using MovieStore.Core.ServiceInterfaces;

namespace MovieStore.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IReviewRepository _reviewRepository;
        public UserController(IUserService userService, IUserRepository userRepository, IReviewRepository reviewRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
            _reviewRepository = reviewRepository;
        }
        // 1.  Purchase a Movie, HttpPost, store that info in the Purchase table
        // http:localhost:12112/User/Purchase -- HttpPost
        // first check whether the user already bought that movie
        // BUY Button in the Movie Details Page will call the above method
        // if user already bought that movie, then replace Buy button with Watch Movie button

        //7/23
        // Filters in ASP.NET [Attributes]
        // Some piece of code that runs either before an controller or action method executes or when some event happens
        // that run before or after specific stages in the Http Pipeline
        // 1. Authorization ---  
        // 2. Action Filter
        // 3. Result Filter
        // 4. Exception filter, but in real world we used Exception middleware to catch exceptions
        // 5. Resource filter

        //  who can call this purchase method???
        // Only Authorized user, user should have entered his un/pw and valid then only we need to execute this method

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Purchase(UserPurchaseRequestModel userPurchaseRequestModel)
        {
            userPurchaseRequestModel.UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var moviePurchased = await _userService.Purchase(userPurchaseRequestModel);
            return LocalRedirect("/user/purchases"); 
        }

        // 2. Get all the Movies Purchased by user, loged in User, take userid from HttpContext and get all the movies
        // and give them to Movie Card partial view
        // http:localhost:12112/User/Purchases -- HttpGet
        [HttpGet]
        [Route("User/Purchases")]
        public async Task<IActionResult> Purchases()
        {
            int getUserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value); //AccountController
            var user = await _userRepository.GetByIdAsync(getUserId);
            //var listOfMovies = await _userService.GetPurchasedMovieIdByUserId(userId);
          
            return View(user.Purchases);
        }

        // 3. Create a Review for a Movie for Loged In user , take userid from HttpContext and save info in Review Table
        // http:localhost:12112/User/review -- HttpPost
        // Review Button will open a popup and ask user to enter a small review in textarea and have him enter
        // movie rating between 1 and 10 and then save
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Review (UserReviewRequestModel userReviewRequestModel)
        {
            userReviewRequestModel.UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            //var userClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var listOfReview = await _reviewRepository.GetReviewsById(userReviewRequestModel.UserId);
            //bool reviewed = false;
            foreach (var rev in listOfReview)
            {
                if (rev.MovieId.Equals(userReviewRequestModel.MovieId))
                {
                    throw new Exception("User already reviewed");
                }
            }

                var addReview = await _userService.Review(userReviewRequestModel);


            //return LocalRedirect("~/");
            return RedirectToAction("Details", "Movies", new { movieId = userReviewRequestModel.MovieId });
        }

        // 4. Get all the Reviews done by loged in User, 
        // http:localhost:12112/User/reviews -- HttpGet
        [Authorize]
        [HttpGet]
        [Route("User/Reviews")]
        public async Task<IActionResult> Reviews ()
        {
            int getUserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var reviews = await _reviewRepository.GetReviewsById(getUserId);
            return View(reviews);
        }
        // 5. Add a Favorite Movie for Loged In User
        // http:localhost:12112/User/Favorite -- HttpPost
        // add another button called favorite, same conecpt as Purchase
        // FontAweomse libbary and use buttons from there

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Favorite (UserFavoriteRequestModel userFavoriteRequestModel)
        {
            
            userFavoriteRequestModel.UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var addFavorite = await _userService.Favorite(userFavoriteRequestModel);
            //return LocalRedirect("Movies/Details/{MovieId}");
            return RedirectToAction("Details", "Movies", new { movieId = userFavoriteRequestModel.MovieId });
        }


        // 6.Check if a particular Movie has been added as Favorite by logedin user 
        // http:localhost:12112/User/{123}/movie/{12}/favorite  HttpGet

        // 7. Remove favorite
        // http:localhost:12112/User/Favorite -- Httpdelete
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UnFavorite (UserFavoriteRequestModel userFavoriteRequestModel)
        {
            userFavoriteRequestModel.UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            await _userService.DeleteFavorite(userFavoriteRequestModel);
            return RedirectToAction("Details", "Movies", new { movieId = userFavoriteRequestModel.MovieId });
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
