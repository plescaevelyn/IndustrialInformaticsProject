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

        public IActionResult AddAlbum(DataContext dataContext, Album album)
        {
            _dataContext
                .Albums
                .Add(album);

            return View();
        }

        public IActionResult UpdateAlbum(DataContext dataContext, Album album)
        {
            _dataContext
                .Albums
                .Update(album);

            return View();
        }

        public IActionResult DeleteAlbum(DataContext dataContext, Album album)
        {
            _dataContext
                .Albums
                .Remove(album);

            return View();
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
