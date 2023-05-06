using System.ComponentModel.DataAnnotations.Schema;

namespace PlayHarmoniez.Models
{
    public class LikedSong
    {
        public int Id { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        [ForeignKey("SongId")]
        public int SongId { get; set; }
        public User User { get; set;}
        public Song Song { get; set; }
    }
}
