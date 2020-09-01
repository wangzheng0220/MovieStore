using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Core.Models;
using MovieStore.Core.Entities;
using MovieStore.Core.Models.Request;
using MovieStore.Core.RepositoryInterfaces;
using MovieStore.Core.ServiceInterfaces;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace MovieStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IFavoriteRepository _favoriteRepository;

        public UserController(IUserService userService, IUserRepository userRepository, IReviewRepository reviewRepository,
            IFavoriteRepository favoriteRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
            _reviewRepository = reviewRepository;
            _favoriteRepository = favoriteRepository;
        }
       // [Authorize]
        [HttpPost]
        [Route("purchase")]
        public async Task<IActionResult> Purchase([FromBody] UserPurchaseRequestModel userPurchaseRequestModel)
        {
            //model binding
            //{
            //    "MovieId" : "model.MovieId",
            //    "UserId" : model.UserId,
            //    "PurchaseDate" : model.PurchaseDate.Value,
            //    "PurchaseNumber" : model.PurchaseNumber.Value
            //}
            //model.UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var checkBought = await _userService.CheckIfPurchasedByUserId(userPurchaseRequestModel.UserId, userPurchaseRequestModel.MovieId);
            if (checkBought == false)
            {
                var purchase = await _userService.Purchase(userPurchaseRequestModel);
                return Ok(purchase);
            }
            else
            {
                return BadRequest("bought this already");
            }
        }

        

        [Authorize]
        // In MVC Authorize attribute will look for Cookie 
        // IN API, it will for JWT in the header
        [HttpGet]
        [Route("purchases/{userId:int}")]
        public async Task<IActionResult> ListOfPurchases(int userId) 
        {
            //model binding
            //{
            //    "userId" : "1889",
            //}
            
            
            var user = await _userRepository.GetByIdAsync(userId);
            if (!user.Purchases.Any())
            {
                return NotFound("No Movies Found!");
            }
            return Ok(user.Purchases);

        }

        [Authorize]
        [HttpGet]
        [Route("favorites/{userId:int}")]
        public async Task<IActionResult> ListOfFavorites(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (!user.Favorites.Any())
            {
                return NotFound("No Movies Found!");
            }
            return Ok(user.Favorites);
        }

        [Authorize]
        [HttpPost]
        [Route("review")]
        public async Task<IActionResult> Review([FromBody] UserReviewRequestModel model)
        {
            //model binding
            //{
            //    "MovieId" : 29,
            //    "UserId" : 1889,
            //    "Rating" : 6,
            //    "ReviewText" : "I like it!"
            //}
            var listOfReviews = await _reviewRepository.GetReviewsById(model.UserId); // list of review entities
            foreach (var rev in listOfReviews)
            {
                if (rev.MovieId.Equals(model.MovieId)) //each user can't review same movie twice
                {
                    return BadRequest("User already reviewed");
                }
            }
            var addReviews = await _userService.Review(model);
            return Ok(addReviews);
        }

        [Authorize]
        [HttpGet]
        [Route("review/{userId:int}")]
        public async Task<IActionResult> GetReviewByUserId(int userId)
        {

            var reviews = await _reviewRepository.GetReviewsById(userId);
            return Ok(reviews);
        }

        [Authorize]
        [HttpGet]
        [Route("{userId:int}")]
        public async Task<IActionResult> GetInfoByUserId(int userId)
        {
            var info = await _userRepository.GetByIdAsync(userId);
            return Ok(info);
        }

        [Authorize]
        [HttpPost]
        [Route("favorite")]
        public async Task<IActionResult> Favorite(UserFavoriteRequestModel model)
        {
            //model binding
            //{
            //    "MovieId" : 29,
            //    "UserId" : 1889
            //}
            var checkFavorite = await _userService.CheckIfUserFavoriteByUserId(model.UserId, model.MovieId);
            if(checkFavorite == false)
            {
                var addFavorite = await _userService.Favorite(model);
                return Ok(addFavorite);
            }
            else
            {
                return BadRequest("Click to unfavorite");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("unfavorite")]
        public async Task<IActionResult> UnFavorite(UserFavoriteRequestModel model)
        {
             await _userService.DeleteFavorite(model);
             return Ok();
        }
    }
}
