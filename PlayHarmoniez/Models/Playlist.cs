namespace PlayHarmoniez.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        public string PlaylistId { get; set; }
        public int UserId { get; set; }
        public List<PlaylistSong> PlaylistSongs { get; set; }
        public User User { get; set; }  
    }
}
