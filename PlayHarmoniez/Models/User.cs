namespace PlayHarmoniez.Models
{
    public class User
    {
        public int Id { get; set; } 
        public string Username { get; set;}
        public string Password { get; set;} 
        public bool AdminCheck { get; set;}
        public LikedSong LikedSong { get; set;} 
    }
}
