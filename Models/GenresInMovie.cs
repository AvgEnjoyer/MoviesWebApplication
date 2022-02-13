using System;
using System.Collections.Generic;

namespace MoviesWebApplication
{
    public partial class GenresInMovie
    {
        public int Id { get; set; }
        public int GenreId { get; set; }
        public int MovieId { get; set; }

        public virtual Genre Genre { get; set; } = null!;
        public virtual Movie Movie { get; set; } = null!;
    }
}
