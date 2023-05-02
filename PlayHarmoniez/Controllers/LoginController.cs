using Microsoft.AspNetCore.Mvc;
using PlayHarmoniez.Models;

namespace PlayHarmoniez.Controllers
{
    public class LoginController : Controller
    {
      
        private readonly ILogin _loginUser;

        public LoginController(ILogin loguser)
        {
            _loginUser = loguser;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string Username, string Password,bool AdminCheck)
        {
            var issuccess = _loginUser.AuthenticateUser(Username, Password,AdminCheck);

            if (issuccess.Result != null)
            {
                ViewBag.Username = string.Format("Successfully logged-in", Username);

                TempData["username"] = "Ahmed";
                return RedirectToAction("Index", "Layout");
            }
            else
            {
                ViewBag.username = string.Format("Login Failed ", Username);
                return View();
            }
        }
    }
}
