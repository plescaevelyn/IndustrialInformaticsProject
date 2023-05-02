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

        public AlbumController(ILogger<HomeController> logger, DataContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }

        public IActionResult Index()
        {
            var songs = _dataContext
                .Songs
                .Include(song => song.Album)
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

        // Method to get all Albums
        [HttpGet]
        public async Task<IActionResult> AlbumList()
        {

            List<Album> albums = await _dataContext.Albums.ToListAsync();
            return View(albums);

        }
        // Method to add album -> working.... kinda? does change database BUTIT DOES ?
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

            await _dataContext.Albums.AddAsync(albumModel);

            await _dataContext.SaveChangesAsync();

            return RedirectToAction("AlbumList");
        }
        // Method to update album -> work in progress
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

        // Method to delete album -> work in progress

        [HttpPost]
        public async Task<IActionResult> DeleteAlbum(Album album)
        {
            var albumModel = await _dataContext.Albums.FindAsync(album.Id);

            if (albumModel != null)
            {
                _dataContext.Remove(album);

                await _dataContext.SaveChangesAsync();
                return RedirectToAction("AlbumList");
            }

            return RedirectToAction("AlbumList");
        }

        public IActionResult GetAlbumById(int id)
        {
            _dataContext
                .Albums
                .SingleOrDefault(album => album.Id == id);

            return View();
        }
    }
}
