using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management_System.Controllers
{
    public class AccountController : Controller
    {
        // Action to Login page
        public IActionResult Login()
        {
            return View();
        }

        // Action to redirect to the register page
        public IActionResult Register()
        {
            return View();
        }
    }
}
