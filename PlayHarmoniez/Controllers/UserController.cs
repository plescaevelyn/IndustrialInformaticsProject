namespace PlayHarmoniez.Controllers
{
    using global::PlayHarmoniez.App_Data;
    using global::PlayHarmoniez.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Diagnostics;

    namespace PlayHarmoniez.Controllers
    {
        public class UserController : Controller
        {
            private readonly ILogger<HomeController> _logger;
            private readonly DataContext _dataContext;

            public UserController(ILogger<HomeController> logger, DataContext dataContext)
            {
                _logger = logger;
                _dataContext = dataContext;
            }

            // TODO: add index if needed

            public IActionResult Privacy()
            {
                return View();
            }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            public IActionResult Login()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Login(string username, string passcode)
            {
                var issuccess = await _dataContext.Users.FirstOrDefaultAsync(authUser => authUser.Username == username && authUser.Password == passcode);

                if (issuccess != null)
                {
                    ViewBag.username = string.Format("Successfully logged-in", username);
                    HttpContext.Session.SetInt32("UserId", issuccess.Id);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.username = string.Format("Login Failed ", username);
                    return View();
                }
            }

            [HttpGet]
            public IActionResult AddUser()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> AddUser(DataContext dataContext, User user)
            {
                User userModel = new User()
                {
                    Id = user.Id,
                    Username = user.Username,
                    Password = user.Password,
                    AdminCheck = user.AdminCheck,
                    LikedSong = user.LikedSong
                };

                await dataContext
                    .Users
                    .AddAsync(user);

                await dataContext
                    .SaveChangesAsync();

                return View("AddUser");
            }

            [HttpGet]
            public IActionResult UpdateUser()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> UpdateUser(DataContext dataContext, User user)
            {
                var userModel = await _dataContext.Users.FindAsync(user.Id);

                if (userModel != null)
                {
                    userModel.Username = user.Username;
                    userModel.Password = user.Password;
                    userModel.AdminCheck = user.AdminCheck;
                    userModel.LikedSong = user.LikedSong;

                    await dataContext.SaveChangesAsync();
                    return RedirectToAction("UserList");
                }

                return RedirectToAction("UserList");
            }

            [HttpGet]
            public IActionResult DeleteUser()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> DeleteUser(DataContext dataContext, User user)
            {
                var userModel = await _dataContext.Users.FindAsync(user.Id);

                if (userModel != null)
                {
                    dataContext.Remove(user);

                    await dataContext.SaveChangesAsync();
                }

                return View("DeleteUser");
            }

            public IActionResult GetUsersById(DataContext dataContext, int id)
            {
                _dataContext
                    .Users
                    .SingleOrDefault(user => user.Id == id);

                return View();
            }
        }
    }

}
