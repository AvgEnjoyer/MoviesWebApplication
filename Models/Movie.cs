using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display (Name ="Ідентифікатор фільму")]
        public int MovieId { get; set; }
        [Display (Name ="Назва фільму") ]
        public string Title { get; set; } = null!;
        [Display(Name = "Опис")]
        public string? Description { get; set; }
        [Display(Name = "Тривалість(хв)")]
        public int LengthMinutes { get; set; }
        
        [Display(Name ="Ідентифікатор режисера")]
        public int? DirectorId { get; set; }

        [Display(Name = "Режисер")]
        public virtual Director? Director { get; set; } = null!;
        public virtual ICollection<ActorsInMovie> ActorsInMovies { get; set; }
        public virtual ICollection<GenresInMovie> GenresInMovies { get; set; }
        public virtual ICollection<MoviesOfUser> MoviesOfUsers { get; set; }
        public virtual ICollection<Score> Scores { get; set; }
    }
}
