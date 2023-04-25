using System.ComponentModel.DataAnnotations;

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
        // public Albums Album { get; set; }
        public string SoundFile { get; set; }
        public string ImageFile { get; set; }
    }
}
