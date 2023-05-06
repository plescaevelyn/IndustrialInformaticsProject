using Microsoft.EntityFrameworkCore;
using PlayHarmoniez.Models;


namespace PlayHarmoniez.App_Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {}

        public DataContext(DbContextOptions options) : base(options)
        {}

        public DbSet<Album> Albums { get; set; }
        public DbSet<LikedSong> LikedSongs { get; set; }
        public DbSet<Playlist> Playlist { get; set; }
        public DbSet<PlaylistSong> PlaylistSongs { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>(entity => {
                entity.HasKey(k => k.Id);
            });
        }


    }
}
