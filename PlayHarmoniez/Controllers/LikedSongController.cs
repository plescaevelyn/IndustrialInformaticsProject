using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayHarmoniez.App_Data;
using PlayHarmoniez.Models;
using System.Diagnostics;

namespace PlayHarmoniez.Controllers
{
    public class LikedSongController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _dataContext;

        public LikedSongController(ILogger<HomeController> logger, DataContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }

        // modify index
        public IActionResult Index()
        {
            var songs = _dataContext
                .Songs
                .Include(song => song.LikedSong)
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

        public IActionResult AddLikedSong(DataContext dataContext, LikedSong likedSong)
        {
            _dataContext
                .LikedSongs
                .Add(likedSong);

            return View();
        }

        public IActionResult UpdateLikedSong(DataContext dataContext, LikedSong likedSong)
        {
            _dataContext
                .LikedSongs
                .Update(likedSong);

            return View();
        }

        public IActionResult DeleteLikedSong(DataContext dataContext, LikedSong likedSong)
        {
            _dataContext
                .LikedSongs
                .Remove(likedSong);

            return View();
        }

        public IActionResult GetLikedSongById(DataContext dataContext, int id)
        {
            _dataContext
                .LikedSongs
                .SingleOrDefault(likedSong => likedSong.Id == id);

            return View();
        }
    }
}
