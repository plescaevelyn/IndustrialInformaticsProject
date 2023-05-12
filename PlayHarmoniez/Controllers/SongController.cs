namespace PlayHarmoniez.Controllers
{
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
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

            public SongController(ILogger<HomeController> logger, DataContext dataContext, BlobServiceClient blobClient)
            {
                _logger = logger;
                _dataContext = dataContext;
                _blobClient = blobClient;
            }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            [HttpGet]
            public async Task<IActionResult> SongsList() { 
            
                List<Song>songs=await _dataContext.Songs.ToListAsync();
                return View(songs);
            
            }

            [HttpGet]
            public IActionResult AddSong()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> AddSong(Song song)
            {
                Song songModel = new Song()
                {
                    Id = song.Id,
                    Title = song.Title,
                    Author = song.Author,
                    PublishData = song.PublishData,
                    Description = song.Description,
                    AlbumId = song.AlbumId,
                    Album = song.Album,
                    SoundFile = song.SoundFile,
                    ImageFile = song.ImageFile,
                    PlaylistSongs = song.PlaylistSongs,
                    LikedSong = song.LikedSong
                };

                await _dataContext
                    .Songs
                    .AddAsync(songModel);

                await _dataContext
                    .SaveChangesAsync();

                return View("SongsList");
            }

            public async Task<string> UploadSong(string name, string containerName, IFormFile file)
            {
                var containerClient = _blobClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(name);

                var httpHeaders = new BlobHttpHeaders()
                {
                    ContentType = file.ContentType
                };

                await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders);
                var blobUrl = blobClient.Uri.AbsoluteUri;

                return blobUrl;
            }

            [HttpGet]
            public async Task<IActionResult> UpdateSong(int Id)
            {
                Song song = await _dataContext.Songs.FirstOrDefaultAsync(e => e.Id == Id);

                if (song == null)
                    return RedirectToAction("SongsList");

                Song updatedSong = new Song()
                {
                    Title = song.Title,
                    Author = song.Author,
                    PublishData = song.PublishData,
                    Description = song.Description,
                    AlbumId = song.AlbumId,
                    Album = song.Album,
                    SoundFile = song.SoundFile,
                    ImageFile = song.ImageFile,
                    PlaylistSongs = song.PlaylistSongs,
                    LikedSong = song.LikedSong,
                };

                return View(updatedSong);
            }

            [HttpPost]
            public async Task<IActionResult> UpdateSong(Song song)
            {
                var songModel = await _dataContext.Songs.FindAsync(song.Id);

                if (songModel != null)
                {
                    songModel.Title = song.Title;
                    songModel.Author = song.Author;
                    songModel.PublishData = song.PublishData;
                    songModel.Description = song.Description;
                    songModel.AlbumId = song.AlbumId;
                    songModel.Album = song.Album;
                    songModel.SoundFile = song.SoundFile;
                    songModel.ImageFile = song.ImageFile;
                    songModel.PlaylistSongs = song.PlaylistSongs;
                    songModel.LikedSong = song.LikedSong;

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
                _dataContext.Songs.Remove(song);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction("SongsList");
            }

            public async Task DeleteImage(string name, string containerName)
            {
                var containerClient = _blobClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(name);

                await blobClient.DeleteIfExistsAsync();
            }

            public IActionResult GetSongById(int id)
            {            
                var song =  _dataContext
                    .Songs
                    .FindAsync(id);

                return View(song);
            }           
        }
    }
}
