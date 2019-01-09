using Microsoft.EntityFrameworkCore;

namespace mrRemoteForKodi_Update_1.Models
{
    public class TvShowContext : DbContext
    {
        public DbSet<TvShow> TvShows { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=TvShow_Update_1.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TvShow>()
                .HasKey(b => b.Tvshowid);
        }
    }

    public class TvShow
    {
        public string Tvshowid { get; set; }

        public string Title { get; set; }
        public string Genre { get; set; }
        public string Year { get; set; }
        public string Rating { get; set; }
        public string Plot { get; set; }
        public string Episode { get; set; }
        public string Season { get; set; }
        public string Fanart { get; set; }
        public string Poster { get; set; }
        public string DateAdded { get; set; }
    }

    public class Season
    {
        public string Label { get; set; }
        public string SeasonNumber { get; set; }
        public string Thumb { get; set; }
    }

    public class Episode
    {
        public string Label { get; set; }
        public string EpisodeId { get; set; }
        public string Plot { get; set; }
        public string Playcount { get; set; }
        public string File { get; set; }
    }
}