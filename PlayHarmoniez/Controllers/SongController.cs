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

            [HttpGet]
            public async Task<IActionResult> SongsList() { 
            
                List<Song>songs=await _dataContext.Songs.ToListAsync();
                return View(songs);
            
            }


            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            [HttpGet]
            public IActionResult AddSong()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> AddSong(DataContext dataContext, Song song)
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

                await dataContext
                    .Songs
                    .AddAsync(songModel);

                await dataContext
                    .SaveChangesAsync();

                return View("AddSong");
            }

            [HttpGet]
            public IActionResult UpdateSong()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> UpdateSong(DataContext dataContext, Song song)
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

                    await dataContext.SaveChangesAsync();
                    return RedirectToAction("SongList");
                }

                return RedirectToAction("SongList");
            }

            [HttpGet]
            public IActionResult DeleteSong()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> DeleteSong(DataContext dataContext, Song song)
            {
                var songModel = await _dataContext.Songs.FindAsync(song.Id);

                if (songModel != null)
                {
                    dataContext.Remove(song);

                    await dataContext.SaveChangesAsync();
                }

                return View("DeleteSong");
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
