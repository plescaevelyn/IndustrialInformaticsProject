
using Microsoft.AspNetCore.Mvc;

using PlayHarmoniez.App_Data;
using PlayHarmoniez.Controllers.PlayHarmoniez.Controllers;
using PlayHarmoniez.Models;
using System.Diagnostics;

namespace PlayHarmoniez.Controllers
{
    public class LikedSongController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _dataContext;
        private readonly SongController _songController;

        public LikedSongController(ILogger<HomeController> logger, DataContext dataContext,SongController songController)
        {
            _logger = logger;
            _dataContext = dataContext;
            _songController = songController;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult DeleteLikedSong()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLikedSong(LikedSong likedSong)
        {
            var likedSongModel = await _dataContext.LikedSongs.FindAsync(likedSong.Id);

            if (likedSongModel != null)
            {
                _dataContext.Remove(likedSong);

                await _dataContext.SaveChangesAsync();
            }

            return View("DeleteLikedSong");
        }
        [HttpGet]
        public async Task<IActionResult> LikedSongList()
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            var songs_id = _dataContext.LikedSongs.Where(e => e.UserId == userId).ToList();
            List<Song> songs = new List<Song>();
            foreach (var likedSong in songs_id)
            {
                var song = _songController.GetSongById(likedSong.SongId);
                songs.Add((Song)song);
            }
            return View(songs);

        }
    }
}
