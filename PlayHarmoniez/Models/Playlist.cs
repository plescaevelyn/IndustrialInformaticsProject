using System.ComponentModel.DataAnnotations.Schema;

namespace PlayHarmoniez.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public List<PlaylistSong> PlaylistSongs { get; set; }
        public User User { get; set; }  
    }
}
