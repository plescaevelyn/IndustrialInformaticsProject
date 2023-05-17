
using Azure.Storage.Blobs;
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
        private readonly BlobServiceClient blobClient;

        public LikedSongController(ILogger<HomeController> logger, DataContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> DeleteLikedSong(int Id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            
            if (_dataContext.LikedSongs == null)
            {
                return Problem("Entity set is null.");
            }
            
            //var likedSong = await _dataContext.LikedSongs.FindAsync(Id,userId);
            //_dataContext.LikedSongs.Remove(likedSong);
            //await _dataContext.SaveChangesAsync();
            
            var songs_id = _dataContext.LikedSongs.Where(e => e.UserId == userId).ToList();
            
            foreach (var likedSong in songs_id)
            {
                if (likedSong.SongId == Id)
                    _dataContext.LikedSongs.Remove(likedSong);
                await _dataContext.SaveChangesAsync();
            }
            
            return RedirectToAction("GetLikedSongs");
        }

        public IActionResult GetLikedSongs()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var songs_id = _dataContext.LikedSongs.Where(e => e.UserId == userId).ToList();
            List<Song> songs = new List<Song>();
            SongController songController = new SongController(_logger, _dataContext, blobClient);
            foreach (var likedSong in songs_id)
            {
                var song = songController.GetSongById(likedSong.SongId);
                if (song != null)
                {
                    songs.Add(song);
                }
            }
            return View(songs);

        }
    }
}