using MovieStore.Core.Entities;
using MovieStore.Core.Models.Request;
using MovieStore.Core.Models.Response;
using MovieStore.Core.RepositoryInterfaces;
using MovieStore.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICryptoService _cryptoService;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IMovieService _movieService;
        private readonly IReviewRepository _reviewRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        public UserService(IUserRepository userRepository, ICryptoService cryptoService, IPurchaseRepository purchaseRepository,
            IMovieService movieService, IReviewRepository reviewRepository, IFavoriteRepository favoriteRepository)
        {
            _userRepository = userRepository;
            _cryptoService = cryptoService;
            _purchaseRepository = purchaseRepository;
            _movieService = movieService;
            _reviewRepository = reviewRepository;
            _favoriteRepository = favoriteRepository;
        }

       

        public async Task<UserRegisterResponseModel> RegisterUser(UserRegisterRequestModel requestModel)
        {
            // Step 1 : Check whether this user already exists in the database
            var dbUser = await _userRepository.GetUserByEmail(requestModel.Email);
            if (dbUser != null)
            {
                // we already have this user(email) in our table
                // return or throw an exception saying Conflict user
                throw new Exception("User already registered, Please try to Login");
            }

            // Step 2 : lets Generate a random unique Salt

            var salt = _cryptoService.GenerateSalt();

            // Never ever craete your own Hashing Algorithm, always use Industry tested/tried Hashing Algorithm
            // Step 3: we  hash the password with the salt created in the above step

            var hashedPassword = _cryptoService.HashPassword(requestModel.Password, salt);

            // craete User object so that we can save it to User Table

            var user = new User //store user registered info into user, user has list of infos
            {
                Email = requestModel.Email,
                Salt = salt,
                HashedPassword = hashedPassword,
                FirstName = requestModel.FirstName,
                LastName = requestModel.LastName
            };

            // Step 4: Save it to Database
            var createdUser = await _userRepository.AddAsync(user);

            var response = new UserRegisterResponseModel 
            {
                Id = createdUser.Id,
                Email = createdUser.Email,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName

            };
            return response; //store user registered info into reponse
        }

        public async Task<UserLoginResponseModel> ValidateUser(string email, string password)
        {
            // Step 1 : Get user record from the databse by email;
            var user = await _userRepository.GetUserByEmail(email);

            if (user == null)
            {
                // user does not even exists
                throw new Exception("Register first, user does not exists");
            }

            // Step 2: we need to hash the password that user entered in the npage with Salt from the database from step1

            var hashedPassword = _cryptoService.HashPassword(password, user.Salt);

            // Step 3 : Compare the databse hashed password with Hashed passowrd genereated in step 2

            if (hashedPassword == user.HashedPassword) //userInputPassword == DataBasePassword
            {
                // user entred right password
                // send some user details
                var response = new UserLoginResponseModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName, 
                    LastName = user.LastName,
                    DateOfBirth = user.DateOfBirth,
                    Email = user.Email
                };
                return response;
            }
            return null;
        }
        public async Task<Purchase> Purchase(UserPurchaseRequestModel userPurchaseRequestModel)
        {
            var movie = await _movieService.GetMovieById(userPurchaseRequestModel.MovieId);
            var purchase = new Purchase
            {//model binding, bind user input with model properties input
                UserId = userPurchaseRequestModel.UserId,
                PurchaseNumber = userPurchaseRequestModel.PurchaseNumber.Value,
                TotalPrice = movie.Price.Value,
                MovieId = userPurchaseRequestModel.MovieId,
                PurchaseDateTime = userPurchaseRequestModel.PurchaseDate.Value,
            };
            return await _purchaseRepository.AddAsync(purchase);
        }

        public async Task<Review> Review(UserReviewRequestModel userReviewRequestModel)
        {
           
            var review = new Review
            {
                MovieId = userReviewRequestModel.MovieId,
                UserId = userReviewRequestModel.UserId,
                Rating = userReviewRequestModel.Rating,
                ReviewText = userReviewRequestModel.ReviewText,
            };
            return await _reviewRepository.AddAsync(review);
        }

        
        public async Task<Favorite> Favorite(UserFavoriteRequestModel userFavoriteRequestModel)
        {
            var favorite = new Favorite
            {
                UserId = userFavoriteRequestModel.UserId,
                MovieId = userFavoriteRequestModel.MovieId,
            };
            return await _favoriteRepository.AddAsync(favorite);
        }

        public async Task DeleteFavorite(UserFavoriteRequestModel userFavoriteRequestModel)
        {
            
            var unFavorite = await _favoriteRepository.ListAsync(f => f.UserId == userFavoriteRequestModel.UserId &&
               f.MovieId == userFavoriteRequestModel.MovieId);
            await _favoriteRepository.DeleteAsync(unFavorite.FirstOrDefault());
             
        }
        // public async Task<IEnumerable<int>> GetPurchasedMovieIdByUserId(int userId)
        //{
        //    return await _userRepository.GetPurchasedMovieIdByUserId(userId);
        //}

    

       public async Task<bool> CheckIfPurchasedByUserId(int userId, int movieId)
        {
            
            var user = await _userRepository.GetByIdAsync(userId); //get single user entity info by userId 
                                                                    // GetByIdAsync already have user and purchase entity joined
            foreach (var purchase in user.Purchases)
            {
                if(purchase.MovieId == movieId)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> CheckIfUserFavoriteByUserId(int userId, int movieId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            foreach (var favorite in user.Favorites)
            {
                if (favorite.MovieId == movieId)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
