using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesWebApplication
{
    public partial class Director
    {
        public Director()
        {
            Movies = new HashSet<Movie>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DirectorId { get; set; }
        [Display(Name ="Режисер")]
        public string FullName { get => $"{Name} {Surname}"; }

        [Display (Name="Ім'я")]
        public string? Name { get; set; }
        [Display(Name = "Фамілія")]
        public string? Surname { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}
