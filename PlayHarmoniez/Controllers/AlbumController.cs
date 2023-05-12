using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayHarmoniez.App_Data;
using PlayHarmoniez.Models;
using System.Diagnostics;

namespace PlayHarmoniez.Controllers
{
    public class AlbumController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _dataContext;
        private readonly BlobServiceClient _blobClient;
        public AlbumController(ILogger<HomeController> logger, DataContext dataContext, BlobServiceClient blobClient)
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
        public async Task<IActionResult> AlbumList()
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
        public async Task<IActionResult> AddAlbum( Album album)
        { 
            Album albumModel = new Album()
            {
                Id = album.Id,
                AlbumName = album.AlbumName,
                AlbumAuthor = album.AlbumAuthor,
                AlbumDescription = album.AlbumDescription,
                AlbumRelease = album.AlbumRelease,
                ImageFile = album.ImageFile,
                Songs = album.Songs
            };

            // UploadImage(Task<string> UploadImage(name, file, containerName);

            await _dataContext.Albums.AddAsync(albumModel);

            await _dataContext.SaveChangesAsync();

            return RedirectToAction("AlbumList");
        }

        public async Task<string> UploadImage(string name, IFormFile file, string containerName)
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
        public async Task<IActionResult> UpdateAlbum(int Id)
        {
            Album album = await _dataContext.Albums.FirstOrDefaultAsync(e=> e.Id ==Id );

            if (album == null)
                return RedirectToAction("AlbumList");
            Album updatedAlbum = new Album()
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
        public async Task<IActionResult> UpdateAlbum(Album updatedAlbum)
        {
            var album = await _dataContext.Albums.FindAsync(updatedAlbum.Id);

            if (album != null)
            {
                album.AlbumName = updatedAlbum.AlbumName;
                album.AlbumAuthor = updatedAlbum.AlbumAuthor;
                album.AlbumDescription = updatedAlbum.AlbumDescription;
                album.AlbumRelease = updatedAlbum.AlbumRelease;
                album.ImageFile = updatedAlbum.ImageFile;
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
            await _dataContext.SaveChangesAsync();
            return RedirectToAction("AlbumList");
        }

        public async Task DeleteImage(string name, string containerName)
        {
            var containerClient = _blobClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(name);

            await blobClient.DeleteIfExistsAsync();
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

    }
}