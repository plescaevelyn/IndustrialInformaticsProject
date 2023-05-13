namespace PlayHarmoniez.Controllers
{
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using global::PlayHarmoniez.App_Data;
    using global::PlayHarmoniez.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.CodeAnalysis.VisualBasic.Syntax;
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
            
                List<Song>songs=await _dataContext.Songs.ToListAsync();
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
                    PlaylistSongs = song.PlaylistSongs,
                    LikedSong = song.LikedSong
                };

                songModel.SoundFile = await UploadSong(songModel.Title, musicContainerName, soundFile);
                songModel.ImageFile = await UploadImage(songModel.Title, musicImageContainerName, imageFile);

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

            public async Task<string> UploadImage(string name, string containerName, IFormFile file)
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
            public async Task<IActionResult> UpdateSong(int Id, IFormFile soundFile, IFormFile imageFile)
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
                    PlaylistSongs = song.PlaylistSongs,
                    LikedSong = song.LikedSong,
                };

                updatedSong.SoundFile = await UploadSong(updatedSong.Title, musicContainerName, soundFile);
                updatedSong.ImageFile = await UploadImage(updatedSong.Title, musicImageContainerName, imageFile);

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
                    songModel.Album = song.Album;
                    songModel.PlaylistSongs = song.PlaylistSongs;
                    songModel.LikedSong = song.LikedSong;

                    songModel.SoundFile = await UploadSong(songModel.Title, musicContainerName, soundFile);
                    songModel.ImageFile = await UploadImage(songModel.Title, musicImageContainerName, imageFile);

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
            public async Task<IActionResult> DeleteSong(int id, string containerName)
            {
                if (_dataContext.Songs == null)
                {
                    return Problem("Entity set is null.");
                }

                var song = await _dataContext.Songs.FindAsync(id);

                var containerClient = _blobClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(song.Title);

                _dataContext.Songs.Remove(song);

                await blobClient.DeleteIfExistsAsync();

                await _dataContext.SaveChangesAsync();
                
                return RedirectToAction("SongsList");
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
