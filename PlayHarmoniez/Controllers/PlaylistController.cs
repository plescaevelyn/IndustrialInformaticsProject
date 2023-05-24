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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> PlaylistList()
        {

            List<Playlist> playlists = await _dataContext.Playlist.ToListAsync();
            return View(playlists);
        }

        [HttpGet]
        public IActionResult AddPlaylist()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPlaylist( Playlist playlist)
        {
            Playlist playlist1 = new Playlist()
            {
                Id = playlist.Id,
                UserId = playlist.UserId,
                Name = playlist.Name,
                Description = playlist.Description,
                PlaylistSongs = playlist.PlaylistSongs,
            };

            await _dataContext
                .Playlist
                .AddAsync(playlist1);

            await _dataContext
                .SaveChangesAsync();

            return View("PlaylistList");
        }

        [HttpGet]
        public async Task <IActionResult> UpdatePlaylist(int Id)
        {

            Playlist newPlaylist = await _dataContext.Playlist.FirstOrDefaultAsync(e => e.Id == Id);
            if (newPlaylist == null)
                return RedirectToAction("GetPlaylistById");
            Playlist updatedPlaylist = new()
            {
                Id = newPlaylist.Id,
                UserId = newPlaylist.UserId,
                Description= newPlaylist.Description,
                Name = newPlaylist.Name,

            };
            return View(updatedPlaylist);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePlaylist(Playlist playlist)
        {
            var playlistModel = await _dataContext.Playlist.FindAsync(playlist.Id);

            if (playlistModel != null)
            {

                playlistModel.UserId = playlist.UserId;
                playlistModel.PlaylistSongs = playlist.PlaylistSongs;
                playlistModel.User = playlist.User;
                playlistModel.Name = playlist.Name;
                playlistModel.Description = playlist.Description;

                await _dataContext.SaveChangesAsync();
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
        public async Task<IActionResult> DeletePlaylist(int Id)
        {
            var playlistModel = await _dataContext.Playlist.FindAsync(Id);

            if (playlistModel != null)
            {
                _dataContext.Remove(playlistModel);

                await _dataContext.SaveChangesAsync();
            }
            return RedirectToAction("PlaylistList");
        }

    }
}
