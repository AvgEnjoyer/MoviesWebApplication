using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MoviesWebApplication
{
    public partial class DBMoviesContext : DbContext
    {
        public DBMoviesContext()
        {
        }

        public DBMoviesContext(DbContextOptions<DBMoviesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Actor> Actors { get; set; } = null!;
        public virtual DbSet<ActorsInMovie> ActorsInMovies { get; set; } = null!;
        public virtual DbSet<Director> Directors { get; set; } = null!;
        public virtual DbSet<Genre> Genres { get; set; } = null!;
        public virtual DbSet<GenresInMovie> GenresInMovies { get; set; } = null!;
        public virtual DbSet<Movie> Movies { get; set; } = null!;
        public virtual DbSet<MoviesOfUser> MoviesOfUsers { get; set; } = null!;
        public virtual DbSet<Score> Scores { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-MF2OKS0;\nDatabase=DBMovies; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actor>(entity =>
            {
                entity.Property(e => e.ActorId).ValueGeneratedNever();

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Surname).HasMaxLength(50);
            });

            modelBuilder.Entity<ActorsInMovie>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Actor)
                    .WithMany(p => p.ActorsInMovies)
                    .HasForeignKey(d => d.ActorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActorsInMovies_Actors");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.ActorsInMovies)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActorsInMovies_Movies");
            });

            modelBuilder.Entity<Director>(entity =>
            {
                entity.Property(e => e.DirectorId).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Surname).HasMaxLength(50);
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.Property(e => e.GenreId).ValueGeneratedNever();

                entity.Property(e => e.Genre1)
                    .HasMaxLength(50)
                    .HasColumnName("Genre");
            });

            modelBuilder.Entity<GenresInMovie>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.GenresInMovies)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GenresInMovies_Genres");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.GenresInMovies)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GenresInMovies_Movies");
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.Property(e => e.MovieId).ValueGeneratedNever();

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.LengthMinutes).HasColumnName("Length(minutes)");

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.Director)
                    .WithMany(p => p.Movies)
                    .HasForeignKey(d => d.DirectorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Movies_Directors");
            });

            modelBuilder.Entity<MoviesOfUser>(entity =>
            {
                entity.ToTable("MoviesOfUser");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.MoviesOfUsers)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MoviesOfUser_Movies");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MoviesOfUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MoviesOfUser_Users");
            });

            modelBuilder.Entity<Score>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Score1).HasColumnName("Score");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.Scores)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Scores_Movies");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Scores)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Scores_Users");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.NickName).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Surname).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
