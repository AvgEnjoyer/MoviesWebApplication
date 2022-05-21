using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesWebApplication
{
    public partial class Director
    {
        public void setNames(string value)
        {
            int i = value.IndexOf(' ');
            Name = value.Substring(0, i);
            Surname = value.Substring(i + 1);
        }
        public Director()
        {
            Movies = new HashSet<Movie>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DirectorId { get; set; }
        [Display(Name = "Режисер")]
        public string FullName => $"{Name} {Surname}";

        [Display (Name="Ім'я")]
        public string? Name { get; set; }
        [Display(Name = "Фамілія")]
        public string? Surname { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}
