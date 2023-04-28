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

        public IActionResult AddPlaylist(DataContext dataContext, Playlist playlist)
        {
            _dataContext
                .Playlist
                .Add(playlist);

            return View();
        }

        public IActionResult UpdatePlaylist(DataContext dataContext, Playlist playlist)
        {
            _dataContext
                .Playlist
                .Update(playlist);

            return View();
        }

        public IActionResult DeletePlaylist(DataContext dataContext, Playlist playlist)
        {
            _dataContext
                .Playlist
                .Remove(playlist);

            return View();
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
