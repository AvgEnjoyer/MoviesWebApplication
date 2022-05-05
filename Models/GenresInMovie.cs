using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesWebApplication
{
    public partial class GenresInMovie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int GenreId { get; set; }
        public int MovieId { get; set; }
        [Display(Name ="Жанри")]
        public virtual Genre? Genre { get; set; } = null!;
        [Display(Name = "Фільми")]
        public virtual Movie? Movie { get; set; } = null!;
    }
}
