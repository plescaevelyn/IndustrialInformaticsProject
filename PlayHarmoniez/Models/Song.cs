using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayHarmoniez.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime PublishData { get; set; }
        public string Description { get; set; }
        public int AlbumId { get; set; }
        public Album Album { get; set; }
        [NotMapped]
        public IFormFile SoundFile { get; set; }
        [NotMapped] 
        public IFormFile ImageFile { get; set; }
        public List<PlaylistSong> PlaylistSongs { get; set; }
        public List<LikedSong> LikedSong { get; set; }
    }
}
