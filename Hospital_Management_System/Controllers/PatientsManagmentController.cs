using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management_System.Controllers
{
    [Authorize]
    public class PatientsManagmentController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
