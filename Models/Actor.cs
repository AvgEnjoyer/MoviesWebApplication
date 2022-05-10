using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviesWebApplication
{
    public partial class Actor
    {
        public Actor()
        {
            ActorsInMovies = new HashSet<ActorsInMovie>();
        }

        public int ActorId { get; set; }
        [Display(Name = "Ім'я")]
        public string Name { get; set; } = null!;
        [Display(Name = "Прізвище")]
        public string Surname { get; set; } = null!;
        [Display(Name = "Дата народження")]
        public DateTime BirthDate { get; set; }
        [Display(Name = "Наявність оскара")]
        public bool? HasOscar { get; set; }
        [Display(Name = "Ім'я актора")]
        public string FullName { get => $"{Name} {Surname}"; }

        public virtual ICollection<ActorsInMovie> ActorsInMovies { get; set; }
    }
}
