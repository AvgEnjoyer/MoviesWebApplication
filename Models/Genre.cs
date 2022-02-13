using System;
using System.Collections.Generic;

namespace MoviesWebApplication
{
    public partial class Genre
    {
        public Genre()
        {
            GenresInMovies = new HashSet<GenresInMovie>();
        }

        public int GenreId { get; set; }
        public string? Genre1 { get; set; }

        public virtual ICollection<GenresInMovie> GenresInMovies { get; set; }
    }
}
