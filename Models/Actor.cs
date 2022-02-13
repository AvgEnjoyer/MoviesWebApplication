using System;
using System.Collections.Generic;

namespace MoviesWebApplication
{
    public partial class Actor
    {
        public Actor()
        {
            ActorsInMovies = new HashSet<ActorsInMovie>();
        }

        public int ActorId { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public bool? HasOscar { get; set; }

        public virtual ICollection<ActorsInMovie> ActorsInMovies { get; set; }
    }
}
