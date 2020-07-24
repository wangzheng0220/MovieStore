using MovieStore.Core.Entities;
using MovieStore.Core.Models.Request;
using MovieStore.Core.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Core.ServiceInterfaces
{
   public interface IUserService
    {
        Task<UserRegisterResponseModel> RegisterUser(UserRegisterRequestModel requestModel);
        Task<UserLoginResponseModel> ValidateUser(string email, string password);

        Task<Purchase> Purchase(UserPurchaseRequestModel userPurchaseRequestModel);
        Task<Review> Review(UserReviewRequestModel userReviewRequestModel);
        Task<Favorite> Favorite(UserFavoriteRequestModel userFavoriteRequestModel);
        Task DeleteFavorite(UserFavoriteRequestModel userFavoriteRequestModel);
        Task<IEnumerable<int>> GetPurchasedMovieIdByUserId(int userId);

        Task<bool> CheckIfPurchasedByUserId(int userId, int movieId);
        Task<bool> CheckIfUserFavoriteByUserId(int userId, int movieId);
      

    }
}
