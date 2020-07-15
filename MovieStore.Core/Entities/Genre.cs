using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MovieStore.Core.Entities
{
    // Genre class represents our Domain Model, we are gonna have all the properties of Genre table Columns
    [Table("Genre")] //change table name
    public class Genre
    {
        // By Convention EF is gonna consider any property in the entity class as Primary key for Id property
        public int Id { get; set; }

        [MaxLength(64)]
        [Required]
        public string Name { get; set; }

        public ICollection<MovieGenre> MovieGenres { get; set; }

    }
}
