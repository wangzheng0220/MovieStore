using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStore.Core.Entities
{
    public class MovieGenre
    {
        public int MovieId { get; set; } //primary key
        public int GenreId { get; set; } //database column

        public virtual Movie Movie { get; set; }
        public virtual Genre Genre { get; set; } //navigation prop not store in property, just in C#

    }
}
