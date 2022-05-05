using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesWebApplication
{
    public partial class Genre
    {
        public Genre()
        {
            GenresInMovies = new HashSet<GenresInMovie>();
        }
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GenreId { get; set; }
        [Display(Name="Жанри")]
        public string? Genre1 { get; set; }

        public virtual ICollection<GenresInMovie> GenresInMovies { get; set; }
    }
}
