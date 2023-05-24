namespace PlayHarmoniez.Controllers
{
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using Azure.Storage.Blobs.Specialized;
    using global::PlayHarmoniez.App_Data;
    using global::PlayHarmoniez.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Diagnostics;

    namespace PlayHarmoniez.Controllers
    {
        public class SongController : Controller
        {
            private readonly ILogger<HomeController> _logger;
            private readonly DataContext _dataContext;
            private readonly BlobServiceClient _blobClient;
            private readonly string musicContainerName;
            private readonly string musicImageContainerName;

            public SongController(ILogger<HomeController> logger, DataContext dataContext, BlobServiceClient blobClient)
            {
                _logger = logger;
                _dataContext = dataContext;
                _blobClient = blobClient;
                musicContainerName = "music";
                musicImageContainerName = "musiccover";
            }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            [HttpGet]
            public async Task<IActionResult> SongsList() { 
            
                List<Song>songs = await _dataContext.Songs.ToListAsync();
                return View(songs);
            }
            [HttpGet]
            public async Task<IActionResult> SongsList_User()
            {

                List<Song> songs = await _dataContext.Songs.ToListAsync();
                return View(songs);
            }

            [HttpGet]
            public IActionResult AddSong()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> AddSong(Song song, IFormFile soundFile, IFormFile imageFile)
            {
                Song songModel = new()
                {
                    Id = song.Id,
                    Title = song.Title,
                    Author = song.Author,
                    PublishData = song.PublishData,
                    Description = song.Description,
                    AlbumId = song.AlbumId,
                    Album = song.Album,
                    SoundFile = await UploadSong(song.Title, musicContainerName, soundFile),
                    ImageFile = await UploadImage(song.Title, musicImageContainerName, imageFile),
                    PlaylistSongs = song.PlaylistSongs,
                    LikedSong = song.LikedSong
                };

                await _dataContext
                    .Songs
                    .AddAsync(songModel);

                await _dataContext
                    .SaveChangesAsync();

                return RedirectToAction("SongsList");
            }

            [HttpPost]
            public async Task<string> UploadSong(string name, string containerName, IFormFile soundFile)
            {
                var containerClient = _blobClient.GetBlobContainerClient(containerName);

                // Create a new block blob
                var blobClient = containerClient.GetBlockBlobClient(name); 

                var httpHeaders = new BlobHttpHeaders()
                {
                    ContentType = soundFile.ContentType
                };

                await blobClient.UploadAsync(soundFile.OpenReadStream(), httpHeaders);
                var blobUrl = blobClient.Uri.AbsoluteUri;

                return blobUrl;
            }

            [HttpPost]
            public async Task<string> UploadImage(string name, string containerName, IFormFile imageFile)
            {
                var containerClient = _blobClient.GetBlobContainerClient(containerName);

                // Create a new block blob
                var blobClient = containerClient.GetBlockBlobClient(name);

                var httpHeaders = new BlobHttpHeaders()
                {
                    ContentType = imageFile.ContentType
                };

                // Upload the file to the block blob
                await blobClient.UploadAsync(imageFile.OpenReadStream(), httpHeaders);

                var blobUrl = blobClient.Uri.AbsoluteUri;

                return blobUrl;
            }

            [HttpGet]
            public async Task<IActionResult> UpdateSong(int Id)
            {
                Song song = await _dataContext.Songs.FirstOrDefaultAsync(e => e.Id == Id);

                if (song == null)
                    return RedirectToAction("SongsList");
                Song updatedSong = new()
                {
                    Title = song.Title,
                    Author = song.Author,
                    PublishData = song.PublishData,
                    Description = song.Description,
                    AlbumId = song.AlbumId,
                    Album = song.Album,
                    SoundFile =song.SoundFile,
                    ImageFile = song.ImageFile,
                    PlaylistSongs = song.PlaylistSongs,
                    LikedSong = song.LikedSong,
                };

                return View(updatedSong);
            }

            [HttpPost]
            public async Task<IActionResult> UpdateSong(Song song, IFormFile imageFile, IFormFile soundFile)
            {
                var songModel = await _dataContext.Songs.FindAsync(song.Id);

                if (songModel != null)
                {
                    songModel.Title = song.Title;
                    songModel.Author = song.Author;
                    songModel.PublishData = song.PublishData;
                    songModel.Description = song.Description;
                    songModel.AlbumId = song.AlbumId;
                    songModel.Album = song.Album;songModel.PlaylistSongs = song.PlaylistSongs;
                    songModel.LikedSong = song.LikedSong;
                    songModel.SoundFile = await UploadSong(song.Title, musicContainerName, soundFile);
                    songModel.ImageFile = await UploadImage(song.Title, musicImageContainerName, imageFile);

                    await _dataContext.SaveChangesAsync();

                    return RedirectToAction("SongsList");
                }

                return RedirectToAction("SongsList");
            }

            [HttpGet]
            public IActionResult DeleteSong()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> DeleteSong(int id)
            {
                if (_dataContext.Songs == null)
                {
                    return Problem("Entity set is null.");
                }

                var song = await _dataContext.Songs.FindAsync(id);

                if (song != null)
                {
                    // deleting the sound file
                    var containerClient = _blobClient.GetBlobContainerClient(musicContainerName);
                    var blobClient = containerClient.GetBlobClient(song.Title);

                    await blobClient.DeleteIfExistsAsync();

                    containerClient = _blobClient.GetBlobContainerClient(musicImageContainerName);

                    // deleting the song image file
                    blobClient = containerClient.GetBlobClient(song.Title);

                    await blobClient.DeleteIfExistsAsync();

                    _dataContext.Songs.Remove(song);

                    await _dataContext.SaveChangesAsync();
                }

                return RedirectToAction("SongsList");
            }
            public async Task<IActionResult> PassSongId(int songId) {

                Song song = await _dataContext.Songs.FirstOrDefaultAsync(e => e.Id == songId);

                if (song == null)
                    return RedirectToAction("SongsList_User");
                else
                return RedirectToAction("Playlist_choose","Playlist",new {Id=songId});
            }
            public async Task<IActionResult> RemoveFromPlaylist(int songId,int playlistId)
            {
                var songs_id = _dataContext.PlaylistSongs.Where(e => e.SongId == songId).ToList();

                foreach (var playlistSong in songs_id)
                {
                    if (playlistSong.PlaylistId == playlistId)
                        _dataContext.PlaylistSongs.Remove(playlistSong);
                    await _dataContext.SaveChangesAsync();
                }
                TempData["playlistId"] = playlistId;
                int plid = playlistId;
                return RedirectToAction("GetPlaylistSongs", "Playlist", new {Id=playlistId});
            }

                public Song? GetSongById(int id)
            {
                var songModel = _dataContext.Songs.Find(id);
                if (songModel != null)
                {
                    return new Song
                    {
                        Id = songModel.Id,
                        Title = songModel.Title,
                        Author = songModel.Author,
                        PublishData = songModel.PublishData,
                        Description = songModel.Description,
                        AlbumId = songModel.AlbumId,
                        SoundFile = songModel.SoundFile,
                        ImageFile = songModel.ImageFile,
                    };
                }
                return null;
            }
        }
    }
}
