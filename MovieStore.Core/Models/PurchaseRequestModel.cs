using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStore.Core.Models
{
    public class PurchaseRequestModel
    {
        public int userId { get; set; }
        public int movieId { get; set; }
    }
}
