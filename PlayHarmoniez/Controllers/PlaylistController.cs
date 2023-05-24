using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayHarmoniez.App_Data;
using PlayHarmoniez.Controllers.PlayHarmoniez.Controllers;
using PlayHarmoniez.Models;
using System.Diagnostics;

namespace PlayHarmoniez.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _dataContext;
        private readonly SongController _songController;
        private readonly BlobServiceClient blobClient;
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
            return View("PlaylistList",playlists);
        }

        [HttpGet]
        public IActionResult AddPlaylist()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPlaylist( Playlist playlist)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            Playlist playlist1 = new Playlist()
            {
                Id = playlist.Id,
                UserId = (int)userId,
                Name = playlist.Name,
                Description = playlist.Description,
                PlaylistSongs = playlist.PlaylistSongs,
            };

            await _dataContext
                .Playlist
                .AddAsync(playlist1);

            await _dataContext
                .SaveChangesAsync();

            return RedirectToAction("PlaylistList");
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
                PlaylistSongs= newPlaylist.PlaylistSongs,

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
        public async Task<IActionResult> Playlist_choose(int Id)
        {
            int id = Id;
            TempData["songId"]=id;
            List<Playlist> playlists = await _dataContext.Playlist.ToListAsync();
            return View("Playlist_choose", playlists);
          
        }
        public async Task<IActionResult> AddPlaylistSong(int playlistId,int songId)
        {
            PlaylistSong playlistsong = new()
            {
                PlaylistId = playlistId,
                SongId = songId,

            };
            await _dataContext.PlaylistSongs.AddAsync(playlistsong);
            await _dataContext.SaveChangesAsync();
            int plid = playlistId;
            return RedirectToAction("GetPlaylistSongs",new { Id=playlistId});
        }

        public async Task<IActionResult> GetPlaylistSongs(int Id) {
           
          
            var playlist = await _dataContext.Playlist.FindAsync(Id);

            TempData["PlaylistName"] = playlist.Name;
            TempData["PlaylistDesc"] = playlist.Description;
            TempData["playlistId"]=playlist.Id;
            
            var songs_id = _dataContext.PlaylistSongs.Where(e => e.PlaylistId == Id).ToList();

            List<Song> songs = new List<Song>();
            SongController songController = new SongController(_logger, _dataContext, blobClient);

            foreach (var songInPlaylist in songs_id)
            {
                var song = songController.GetSongById(songInPlaylist.SongId);
                if (song != null)
                {
                    songs.Add(song);
                }
            }
            return View(songs);
        
        }
    }
}
