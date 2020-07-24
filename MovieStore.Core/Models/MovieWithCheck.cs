using MovieStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStore.Core.Models
{
    public class MovieWithCheck
    {
        public Movie Movie{ get; set; }

        public Boolean Purchased { get; set; }

        public Boolean Favorited { get; set; }
    }
}
