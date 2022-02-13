using System;
using System.Collections.Generic;

namespace MoviesWebApplication
{
    public partial class Movie
    {
        public Movie()
        {
            ActorsInMovies = new HashSet<ActorsInMovie>();
            GenresInMovies = new HashSet<GenresInMovie>();
            MoviesOfUsers = new HashSet<MoviesOfUser>();
            Scores = new HashSet<Score>();
        }

        public int MovieId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int LengthMinutes { get; set; }
        public int DirectorId { get; set; }

        public virtual Director Director { get; set; } = null!;
        public virtual ICollection<ActorsInMovie> ActorsInMovies { get; set; }
        public virtual ICollection<GenresInMovie> GenresInMovies { get; set; }
        public virtual ICollection<MoviesOfUser> MoviesOfUsers { get; set; }
        public virtual ICollection<Score> Scores { get; set; }
    }
}
