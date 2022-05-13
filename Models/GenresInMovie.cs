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
        [Display(Name = "Жанр")]
        public int GenreId { get; set; }
        [Display(Name = "Фільм")]
        public int MovieId { get; set; }
  
        public virtual Genre? Genre { get; set; } = null!;
        
        public virtual Movie? Movie { get; set; } = null!;
    }
}
