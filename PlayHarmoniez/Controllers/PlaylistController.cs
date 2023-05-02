using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayHarmoniez.App_Data;
using PlayHarmoniez.Models;
using System.Diagnostics;

namespace PlayHarmoniez.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _dataContext;

        public PlaylistController(ILogger<HomeController> logger, DataContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }

        // TODO: modify this
        public IActionResult Index()
        {
            var songs = _dataContext
                .Songs
                .Include(song => song.PlaylistSongs)
                .ToList();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult AddPlaylist()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPlaylist(DataContext dataContext, Playlist playlist)
        {
            Playlist playlist1 = new Playlist()
            {
                Id = playlist.Id,
                UserId = playlist.UserId,
                PlaylistSongs = playlist.PlaylistSongs,
                User = playlist.User,
            };

            await dataContext
                .Playlist
                .AddAsync(playlist1);

            await dataContext
                .SaveChangesAsync();

            return View("AddPlaylist");
        }

        [HttpGet]
        public IActionResult UpdatePlaylist()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePlaylist(DataContext dataContext, Playlist playlist)
        {
            var playlistModel = await _dataContext.Playlist.FindAsync(playlist.Id);

            if (playlistModel != null)
            {
                playlistModel.PlaylistId = playlist.PlaylistId;
                playlistModel.UserId = playlist.UserId;
                playlistModel.PlaylistSongs = playlist.PlaylistSongs;
                playlistModel.User = playlist.User;

                await dataContext.SaveChangesAsync();
                return RedirectToAction("PlaylistList");
            }

            return RedirectToAction("PlaylistList");
        }

        [HttpGet]
        public IActionResult DeletePlaylist()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeletePlaylist(DataContext dataContext, Playlist playlist)
        {
            var playlistModel = await _dataContext.LikedSongs.FindAsync(playlist.Id);

            if (playlistModel != null)
            {
                dataContext.Remove(playlist);

                await dataContext.SaveChangesAsync();
            }

            return View("DeletePlaylist");
        }

        public IActionResult GetPlaylistById(DataContext dataContext, int id)
        {
            _dataContext
                .Playlist
                .SingleOrDefault(playlist => playlist.Id == id);

            return View();
        }
    }
}
