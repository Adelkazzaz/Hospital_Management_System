using demo.Models;
using Microsoft.AspNetCore.Mvc;

namespace testDBConection.Controllers
{
    public class StaffController : Controller
    {
        HMSEntites context = new HMSEntites();
        public IActionResult Index()
        {
            
            List<Staff> staffs = context.Staffs.ToList();

            return View("staff", staffs);
        }
    }
}
