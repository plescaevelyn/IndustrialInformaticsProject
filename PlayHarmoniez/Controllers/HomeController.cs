using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayHarmoniez.App_Data;
using PlayHarmoniez.Models;
using System.Diagnostics;

namespace PlayHarmoniez.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _dataContext;

        public HomeController(ILogger<HomeController> logger, DataContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext; 
        }

        public IActionResult Index()
        {            
            return View();
        }
        public IActionResult HomePage_Admin() {
            return View();
        }
        public IActionResult HomePage_User()
        {
            return View();
        }
        public IActionResult LandingPage()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}