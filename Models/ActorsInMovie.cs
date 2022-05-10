using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviesWebApplication
{
    public partial class ActorsInMovie
    {
        public int Id { get; set; }
        [Display(Name="Актор")]
        public int ActorId { get; set; }
        [Display(Name="Фільм")]
        public int MovieId { get; set; }

        public virtual Actor? Actor { get; set; } = null!;
        public virtual Movie? Movie { get; set; } = null!;
    }
}
