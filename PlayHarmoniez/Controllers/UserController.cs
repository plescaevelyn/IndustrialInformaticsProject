namespace PlayHarmoniez.Controllers
{
    using global::PlayHarmoniez.App_Data;
    using global::PlayHarmoniez.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

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
                    if (issuccess.AdminCheck == true)
                        return RedirectToAction("HomePage_Admin", "Home");
                    else return RedirectToAction("HomePage_User","Home");
                }
                else
                {
                    ViewBag.username = string.Format("Login Failed ");
                    return View();
                }
            }

            [HttpGet]
            public IActionResult AddUser()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> AddUser(String passcode,String passcode2,String username)
            {

                var userExists = await _dataContext.Users.FirstOrDefaultAsync(authUser => authUser.Username == username);
                if (userExists != null)
                {
                    TempData["User_err"] = "User already exists";
                    return RedirectToAction("AddUser","User");
                }
                else
                {
                    if (passcode == passcode2)
                    {
                        User userModel = new User()
                        {
                            Username = username,
                            Password = passcode,
                            AdminCheck = false,
                           
                        };

                        await _dataContext.Users.AddAsync(userModel);
                        await _dataContext.SaveChangesAsync();
                        return RedirectToAction("Login", "User");

                    }
                    else {
                        TempData["Pass_err"] = "Passwords do not match";
                        return RedirectToAction("AddUser", "User");
                    }
                    
                }
            }
            [HttpPost]
            public async Task<IActionResult> UpdateUser(User user)
            {
                var userModel = await _dataContext.Users.FindAsync(user.Id);
                if (userModel != null)
                {
                    userModel.Username = user.Username;
                    userModel.Password = user.Password;
                    userModel.AdminCheck = false;
                    await _dataContext.SaveChangesAsync();
                }
                return RedirectToAction("GetUserById", "User");
            }
            [HttpGet]
            public IActionResult DeleteUser()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> DeleteUser(int id)
            {
                var user = await _dataContext.Users.FindAsync(id);

                if (user != null)
                {
                    _dataContext.Remove(user);
                    await _dataContext.SaveChangesAsync();
                }
                return RedirectToAction("Login", "User");
            }

            public IActionResult GetUserById()
            {
                int? userId = HttpContext.Session.GetInt32("UserId");

                User user = _dataContext.Users.Find(userId);
                return View(user);

            }
        }
    }

}
