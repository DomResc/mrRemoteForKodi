using Microsoft.EntityFrameworkCore;

namespace mrRemoteForKodi_Update_1.Models
{
    public class MovieContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Movie_Update_1.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .HasKey(b => b.Movieid);
        }
    }

    public class Movie
    {
        public string Movieid { get; set; }

        public string Title { get; set; }
        public string Genre { get; set; }
        public string Year { get; set; }
        public string Rating { get; set; }
        public string Director { get; set; }
        public string Tagline { get; set; }
        public string Plot { get; set; }
        public string Writer { get; set; }
        public string Runtime { get; set; }
        public string Fanart { get; set; }
        public string Poster { get; set; }
        public string DateAdded { get; set; }
        public string ImdbNumber { get; set; }
        public string File { get; set; }
    }
}