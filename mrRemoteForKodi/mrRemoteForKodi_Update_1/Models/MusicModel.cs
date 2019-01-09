using Microsoft.EntityFrameworkCore;

namespace mrRemoteForKodi_Update_1.Models
{
    public class ArtistContext : DbContext
    {
        public DbSet<Artist> Artists { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Artist_Update_1.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artist>()
                .HasKey(b => b.ArtistId);
        }
    }

    public class Artist
    {
        public string ArtistId { get; set; }

        public string Label { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string Poster { get; set; }
        public string Fanart { get; set; }
    }

    public class Album
    {
        public string Label { get; set; }
        public string AlbumId { get; set; }
        public string Poster { get; set; }
    }

    public class Song
    {
        public string Label { get; set; }
        public string SongId { get; set; }
        public string Track { get; set; }
    }
}
