using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayHarmoniez.App_Data;
using PlayHarmoniez.Controllers.PlayHarmoniez.Controllers;
using PlayHarmoniez.Models;
using System.Diagnostics;
using PlayHarmoniez.Controllers.PlayHarmoniez.Controllers;

namespace PlayHarmoniez.Controllers
{
    public class AlbumController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _dataContext;
        private readonly BlobServiceClient _blobClient;
        private readonly string albumContainerName;
        
        public AlbumController(ILogger<HomeController> logger, DataContext dataContext, BlobServiceClient blobClient)
        {
            _logger = logger;
            _dataContext = dataContext;
            _blobClient = blobClient;
            albumContainerName = "albumcover";
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> AlbumList()
        {
            List<Album> albums = await _dataContext.Albums.ToListAsync();
            return View(albums);
        }
        [HttpGet]
        public async Task<IActionResult> AlbumList_User()
        {
            List<Album> albums = await _dataContext.Albums.ToListAsync();
            return View(albums);
        }

        [HttpGet]
        public IActionResult AddAlbum()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAlbum(Album album, IFormFile imageFile)
        {
            Album albumModel = new()
            {
                Id = album.Id,
                AlbumName = album.AlbumName,
                AlbumAuthor = album.AlbumAuthor,
                AlbumDescription = album.AlbumDescription,
                AlbumRelease = album.AlbumRelease,
                ImageFile = await UploadImage(album.AlbumName, albumContainerName, imageFile),
                Songs = album.Songs
            };

            await _dataContext.Albums.AddAsync(albumModel);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction("AlbumList");
        }

        public async Task<string> UploadImage(string name, string containerName, IFormFile imageFile)
        {
            var containerClient = _blobClient.GetBlobContainerClient(containerName);

            var blobClient = containerClient.GetBlockBlobClient(name);

            var httpHeaders = new BlobHttpHeaders()
            {
                ContentType = imageFile.ContentType
            };

            await blobClient.UploadAsync(imageFile.OpenReadStream(), httpHeaders);
            var blobUrl = blobClient.Uri.AbsoluteUri;

            return blobUrl;
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAlbum(int Id)
        {
            Album album = await _dataContext.Albums.FirstOrDefaultAsync(e=> e.Id ==Id );

            if (album == null)
                return RedirectToAction("AlbumList");

            Album updatedAlbum = new()
            {
                AlbumName = album.AlbumName,
                AlbumAuthor = album.AlbumAuthor,
                AlbumDescription = album.AlbumDescription,
                AlbumRelease = album.AlbumRelease,
                ImageFile = album.ImageFile,
                Songs = album.Songs,
            };

            return View(updatedAlbum);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAlbum(Album updatedAlbum, IFormFile imageFile)
        {
            var album = await _dataContext.Albums.FindAsync(updatedAlbum.Id);

            if (album != null)
            {
                album.AlbumName = updatedAlbum.AlbumName;
                album.AlbumAuthor = updatedAlbum.AlbumAuthor;
                album.AlbumDescription = updatedAlbum.AlbumDescription;
                album.AlbumRelease = updatedAlbum.AlbumRelease;
                album.ImageFile = await UploadImage(album.AlbumName, albumContainerName, imageFile);
                album.Songs = updatedAlbum.Songs;

                await _dataContext.SaveChangesAsync();
                return RedirectToAction("AlbumList");
            }

            return RedirectToAction("AlbumList");
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            if (_dataContext.Albums == null)
            {
                return Problem("Entity set is null.");
            }

            var album = await _dataContext.Albums.FindAsync(id);
            
            _dataContext.Albums.Remove(album);

            var containerClient = _blobClient.GetBlobContainerClient(albumContainerName);
            var blobClient = containerClient.GetBlobClient(album.AlbumName);

            await blobClient.DeleteIfExistsAsync();

            await _dataContext.SaveChangesAsync();
            
            return RedirectToAction("AlbumList");
        }

        [HttpGet]
        public async Task<IActionResult> GetAlbumByName(string name)
        {
            Album album = await _dataContext.Albums.FirstOrDefaultAsync(e => e.AlbumName == name);
            //TODO: adding an error window
            if (album == null)
                return RedirectToAction("AddAlbum");

            return View(album);
        }
        public async Task<IActionResult> GetSongsOfAlbum(int id)
        {

            var album = await _dataContext.Albums.FindAsync(id);

            TempData["albumCover"] = album.ImageFile;
            TempData["albumName"] = album.AlbumName;
            TempData["albumAuthor"] = album.AlbumAuthor;
            TempData["albumRelease"] = album.AlbumRelease;
            TempData["albumDesc"] = album.AlbumDescription;


            var songs_id = _dataContext.Songs.Where(e => e.AlbumId == id).ToList();
            List<Song> songs = new List<Song>();
            SongController songController = new SongController(_logger, _dataContext, _blobClient);
            foreach (var albumSong in songs_id)
            {
                var song = songController.GetSongById(albumSong.Id);
                if (song != null)
                {
                    songs.Add(song);
                }
            }
            return View(songs);

        }
        public async Task<IActionResult> GetSongsOfAlbum_User(int id)
        {
            var album = await _dataContext.Albums.FindAsync(id);

            TempData["albumCover"] = album.ImageFile;
            TempData["albumName"] = album.AlbumName;
            TempData["albumAuthor"] =album.AlbumAuthor;
            TempData["albumRelease"] = album.AlbumRelease;
            TempData["albumDesc"] = album.AlbumDescription;


            var songs_id = _dataContext.Songs.Where(e => e.AlbumId == id).ToList();
            List<Song> songs = new List<Song>();
            SongController songController = new SongController(_logger, _dataContext, _blobClient);
            foreach (var albumSong in songs_id)
            {
                var song = songController.GetSongById(albumSong.Id);
                if (song != null)
                {
                    songs.Add(song);
                }
            }
            return View(songs);

        }

    }
}