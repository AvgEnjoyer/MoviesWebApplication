using System;
using System.Collections.Generic;

namespace MoviesWebApplication
{
    public partial class ActorsInMovie
    {
        public int Id { get; set; }
        public int ActorId { get; set; }
        public int MovieId { get; set; }

        public virtual Actor Actor { get; set; } = null!;
        public virtual Movie Movie { get; set; } = null!;
    }
}
