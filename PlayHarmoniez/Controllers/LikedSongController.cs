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

        [HttpGet]
        public IActionResult AddLikedSong()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddLikedSong(DataContext dataContext, LikedSong likedSong)
        {
            LikedSong likedSong1 = new LikedSong()
            {
                Id = likedSong.Id,
                UserId = likedSong.UserId,
                SongId = likedSong.SongId,
                User = likedSong.User,
                Song = likedSong.Song
            };

            await dataContext
                .LikedSongs
                .AddAsync(likedSong1);

            await dataContext
                .SaveChangesAsync();

            return View("AddLikedSong");
        }

        [HttpGet]
        public IActionResult UpdateLikedSong()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLikedSong(DataContext dataContext, LikedSong likedSong)
        {
            var likedSongModel = await _dataContext.LikedSongs.FindAsync(likedSong.Id);

            if (likedSongModel != null)
            {
                likedSongModel.UserId = likedSong.UserId;
                likedSongModel.SongId = likedSong.SongId;
                likedSongModel.User = likedSong.User;
                likedSongModel.Song = likedSong.Song;

                await dataContext.SaveChangesAsync();
                return RedirectToAction("LikedSongList");
            }

            return RedirectToAction("LikedSongList");
        }

        [HttpGet]
        public IActionResult DeleteLikedSong()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLikedSong(DataContext dataContext, LikedSong likedSong)
        {
            var likedSongModel = await _dataContext.LikedSongs.FindAsync(likedSong.Id);

            if (likedSongModel != null)
            {
                dataContext.Remove(likedSong);

                await dataContext.SaveChangesAsync();
            }

            return View("DeleteLikedSong");
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
