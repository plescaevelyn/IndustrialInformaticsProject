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

            public IActionResult AddUser(DataContext dataContext, User user)
            {
                _dataContext
                    .Users
                    .Add(user);

                return View();
            }

            public IActionResult UpdateUser(DataContext dataContext, User user)
            {
                _dataContext
                    .Users
                    .Update(user);

                return View();
            }

            public IActionResult DeleteUser(DataContext dataContext, User user)
            {
                _dataContext
                    .Users
                    .Remove(user);

                return View();
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
