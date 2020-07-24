using MovieStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStore.Core.Models.Request
{
    public class UserPurchaseRequestModel
    {
        public UserPurchaseRequestModel()
        {
            PurchaseDate = DateTime.Now;
            PurchaseNumber = Guid.NewGuid();
        }
        public int MovieId { get; set; } //single movie
        public int UserId { get; set; }

        public Guid? PurchaseNumber { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime? PurchaseDate { get; set; }
        //public Boolean Purchased { get; set; }
    }
}
