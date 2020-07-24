using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStore.Core.Models.Request
{
    public class UserReviewRequestModel
    {
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public decimal Rating { get; set; }
        public string ReviewText { get; set; }
    }
}
