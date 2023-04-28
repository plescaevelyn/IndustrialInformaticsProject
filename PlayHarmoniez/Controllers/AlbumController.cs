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

        [HttpGet]
        public IActionResult AddAlbum()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAlbum(DataContext dataContext, Album album)
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

            await dataContext
                .Albums
                .AddAsync(albumModel);

            await dataContext
                .SaveChangesAsync();

            return View("AddAlbum");
        }

        [HttpGet]
        public IActionResult UpdateAlbum()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAlbum(DataContext dataContext, Album album)
        {
            var albumModel = await _dataContext.Albums.FindAsync(album.Id);

            if (albumModel != null)
            {
                albumModel.AlbumName = album.AlbumName;
                albumModel.AlbumAuthor = album.AlbumAuthor;
                albumModel.AlbumDescription = album.AlbumDescription;
                albumModel.AlbumRelease = album.AlbumRelease;
                albumModel.ImageFile = album.ImageFile;
                albumModel.Songs = album.Songs;

                await dataContext.SaveChangesAsync();
                return RedirectToAction("AlbumList");
            }

            return RedirectToAction("AlbumList");
        }

        [HttpGet]
        public IActionResult DeleteAlbum()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAlbum(DataContext dataContext, Album album)
        {
            var albumModel = await _dataContext.Albums.FindAsync(album.Id);

            if (albumModel != null)
            {
                dataContext.Remove(album);

                await dataContext.SaveChangesAsync();
            }

            return View("DeleteAlbum");
        }

        public IActionResult GetAlbumById(DataContext dataContext, int id)
        {
            _dataContext
                .Albums
                .SingleOrDefault(album => album.Id == id);

            return View();
        }
    }
}
