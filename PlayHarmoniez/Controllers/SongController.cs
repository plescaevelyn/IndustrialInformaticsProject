namespace PlayHarmoniez.Controllers
{
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

            public SongController(ILogger<HomeController> logger, DataContext dataContext)
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

            public IActionResult AddSong(DataContext dataContext, Song song)
            {
                _dataContext
                    .Songs
                    .Add(song);

                return View();
            }

            public IActionResult UpdateSong(DataContext dataContext, Song song)
            {
                _dataContext
                    .Songs
                    .Update(song);

                return View();
            }

            public IActionResult DeleteSong(DataContext dataContext, Song song)
            {
                _dataContext
                    .Songs
                    .Remove(song);

                return View();
            }

            public IActionResult GetSongById(DataContext dataContext, int id)
            {
                _dataContext
                    .Songs
                    .SingleOrDefault(song => song.Id == id);

                return View();
            }
        }
    }

}
