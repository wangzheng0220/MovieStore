using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStore.Core.Models.Request
{
    public class UserFavoriteRequestModel
    {
        //public int Id { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
    }
}
