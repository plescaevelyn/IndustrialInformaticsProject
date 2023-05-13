using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayHarmoniez.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string AlbumName { get; set; }
        public string AlbumAuthor { get; set; }
        public DateTime AlbumRelease { get; set; }
        public string AlbumDescription { get; set; }
        public string ImageFile { get; set; }
        public List<Song> Songs { get; set; }
    }
}