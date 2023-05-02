using System.ComponentModel.DataAnnotations;

namespace PlayHarmoniez.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Username")]
        [Display(Name = "Please Enter Username")]
        public string  Username { get; set;}
        [Required(ErrorMessage = "Please Enter Password")]
        [Display(Name = "Please Enter Password")]
        public string  Password { get; set;} 
        public bool AdminCheck { get; set;}
        public LikedSong LikedSong { get; set;} 
    }
}
