using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MoviesWebApplication
{
    public partial class Director
    {
        public Director()
        {
            Movies = new HashSet<Movie>();
        }
        public int DirectorId { get; set; }
        [Display(Name ="Режисер")]
        public string FullName { get { return string.Format("{0} {1}", Name, Surname); } }

        [Display (Name="Ім'я")]
        public string? Name { get; set; }
        [Display(Name = "Фамілія")]
        public string? Surname { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}
