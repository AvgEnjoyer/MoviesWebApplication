using System;
using System.Collections.Generic;

namespace MoviesWebApplication
{
    public partial class User
    {
        public User()
        {
            MoviesOfUsers = new HashSet<MoviesOfUser>();
            Scores = new HashSet<Score>();
        }

        public int UserId { get; set; }
        public string NickName { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual ICollection<MoviesOfUser> MoviesOfUsers { get; set; }
        public virtual ICollection<Score> Scores { get; set; }
    }
}
