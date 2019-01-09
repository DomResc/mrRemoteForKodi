using Microsoft.EntityFrameworkCore;

namespace mrRemoteForKodi_Update_1.Models
{
    public class RemoteContext : DbContext
    {
        public DbSet<Remote> Remotes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Remote.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Remote>()
                .HasKey(b => b.Name);
        }
    }

    public class Remote
    {
        public string Name { get; set; }

        public string Host { get; set; }
        public string Port { get; set; }
        public string User { get; set; }
        public string Pass { get; set; }
        public bool Fav { get; set; }

        public string WolMac { get; set; }
        public string WolMask { get; set; }
        public string WolPort { get; set; }
    }
}