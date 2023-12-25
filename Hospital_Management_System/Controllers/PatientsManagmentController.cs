using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management_System.Controllers
{
	public class PatientsManagmentController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
