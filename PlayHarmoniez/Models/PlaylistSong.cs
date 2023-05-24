using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayHarmoniez.Models
{
    public class PlaylistSong
    {
        public int Id { get; set; }
        [ForeignKey("PlaylistId")]
        public int PlaylistId { get; set; }
        [ForeignKey("SongId")]
        public int SongId { get; set; }
        public Playlist Playlist { get; set; }
        public Song Song { get; set; }
    }
}
