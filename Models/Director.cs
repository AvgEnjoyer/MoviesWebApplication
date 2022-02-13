using System;
using System.Collections.Generic;

namespace MoviesWebApplication
{
    public partial class Director
    {
        public Director()
        {
            Movies = new HashSet<Movie>();
        }

        public int DirectorId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}
