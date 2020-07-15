using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStore.Core.Entities
{
    // One Trailer belongs to single Movie, but one Movie can have multiple Trailers
    public class Trailer
    {
        public int Id { get; set; }
        public string TrailerUrl { get; set; }
        public string Name { get; set; }
        public int MovieId { get; set; }

        // If someone gave you Trailer Id and you wanna find that Movie details, then this property will be useful
        // Navigation property
        public Movie Movie { get; set; }
    }
}
